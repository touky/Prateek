namespace Prateek.Runtime.AppContentFramework.Unity.Addressables
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net.Mime;
    using System.Text;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Loader.Enums;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using UnityEngine;

    public class LocalRegistryServant
        : ContentRegistryServant
    {
        private class ContentToc
        {
            public List<string> folders = new List<string>();
            public List<string> ignoredFolders = new List<string>();
            public List<string> ignoredFiles = new List<string>();
        }

        private const string EXTRA_CONTENT = "../ExtraContent";
        private const string EXTRA_CONTENT_TOC = "ContentLookUp.json";
        
        private const string SEARCH_PATTERN = "*.*";
        private const char WILDCARD = '*';
        private static readonly string WILDCARD_S = $"{WILDCARD}";
        private static readonly char[] WILDCARD_A = {WILDCARD};

        private const string ERROR_FILE = "ErrorLog.txt";

        #region Fields
        private bool addressSystemInitialized = false;
        private bool workPending = false;

        private string contentPath = string.Empty;
        private string contentTocPath = string.Empty;
        private ContentToc contentToc = null;
        private List<DirectoryInfo> lookUpInfos = new List<DirectoryInfo>();
        private Dictionary<string, FileInfo> pathToFileInfos = new Dictionary<string, FileInfo>();
        #endregion

        #region Properties
        public override bool IsAlive
        {
            get { return base.IsAlive && addressSystemInitialized; }
        }
        #endregion

        #region Class Methods
        public override void ExecutingState(State state)
        {
            switch (state)
            {
                case State.Startup:
                {
                    contentPath = Path.Combine(Application.dataPath, EXTRA_CONTENT);
                    var contentInfo = new DirectoryInfo(contentPath);
                    if (!contentInfo.Exists)
                    {
                        break;
                    }

                    lookUpInfos.Add(contentInfo);

                    contentTocPath = Path.Combine(contentPath, EXTRA_CONTENT_TOC);

                    var builder = (StringBuilder)null;
                    var contentTocInfo = new FileInfo(contentTocPath);
                    if (contentTocInfo.Exists)
                    {
                        var json = File.ReadAllText(contentTocInfo.FullName);
                        contentToc = JsonUtility.FromJson<ContentToc>(json);
                        if (contentToc != null && contentToc.folders != null && contentToc.folders.Count > 0)
                        {
                            foreach (var folder in contentToc.folders)
                            {
                                var directoryInfo = new DirectoryInfo(folder);
                                if (!directoryInfo.Exists)
                                {
                                    directoryInfo = new DirectoryInfo(Path.Combine(Application.dataPath, folder));
                                    if (!directoryInfo.Exists)
                                    {
                                        if (builder == null)
                                        {
                                            builder = new StringBuilder();
                                            builder.AppendLine("Some errors were found");
                                        }

                                        builder.AppendLine($"- Couldn't resolve {folder}");
                                        continue;
                                    }
                                }

                                lookUpInfos.Add(directoryInfo);
                            }
                        }
                    }
                    else
                    {
                        contentToc = new ContentToc();
                        var json = JsonUtility.ToJson(contentToc, true);
                        File.WriteAllText(contentTocInfo.FullName, json);
                    }

                    if (builder != null && builder.Length > 0)
                    {
                        var errorPath = Path.Combine(contentPath, ERROR_FILE);
                        File.WriteAllText(errorPath, builder.ToString());
                    }

                    workPending = lookUpInfos.Count > 0;

                    break;
                }
                case State.StartWork:
                {
                    pathToFileInfos.Clear();
                    break;
                }
                case State.Working:
                {
                    foreach (var lookUpInfo in lookUpInfos)
                    {
                        var filePaths = Directory.GetFiles(lookUpInfo.FullName, SEARCH_PATTERN, SearchOption.AllDirectories);
                        foreach (var filePath in filePaths)
                        {
                            var fileInfo = new FileInfo(filePath);
                            if (ShouldIgnore(fileInfo))
                            {
                                continue;
                            }

                            var relativePath = fileInfo.FullName.Replace(lookUpInfo.FullName, string.Empty);
                            if (pathToFileInfos.ContainsKey(relativePath))
                            {
                                pathToFileInfos.Remove(relativePath);
                            }

                            pathToFileInfos.Add(relativePath, fileInfo);
                        }
                    }

                    foreach (var relativePath in pathToFileInfos.Keys)
                    {
                        ValidatePath(relativePath);
                    }

                    break;
                }
            }

            base.ExecutingState(state);
        }

        protected override ContentLoader GetNewContentLoader(string path)
        {
            return new LocalContentLoader(path, pathToFileInfos[path]);
        }

        public override void SetupDebugDocument(DebugMenuDocument document)
        {
            //var section = new AddressableRegistrySection(this, "Addressable Servant");

            //document.AddSections(section);
        }

        private bool ShouldIgnore(FileInfo fileInfo)
        {
            if (contentToc == null)
            {
                return false;
            }

            foreach (var folder in contentToc.ignoredFolders)
            {
                var splits = (string[]) null;
                if (folder.Contains(WILDCARD_S))
                {
                    splits = folder.Split(WILDCARD_A, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    splits = new[] { folder };
                }

                var directoryInfo = fileInfo.Directory;
                while (directoryInfo != null)
                {
                    var index    = Const.INDEX_NONE;
                    var allValid = true;
                    foreach (var split in splits)
                    {
                        var splitIndex = directoryInfo.Name.IndexOf(split, StringComparison.InvariantCulture);
                        if (splitIndex == Const.INDEX_NONE || splitIndex <= index)
                        {
                            allValid = false;
                            break;
                        }

                        index = splitIndex;
                    }

                    if (allValid)
                    {
                        return true;
                    }
                }
            }

            foreach (var file in contentToc.ignoredFiles)
            {
                var splits = (string[]) null;
                if (file.Contains(WILDCARD_S))
                {
                    splits = file.Split(WILDCARD_A, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    splits = new[] { file };
                }

                var index    = Const.INDEX_NONE;
                var allValid = true;
                foreach (var split in splits)
                {
                    var splitIndex = fileInfo.Name.IndexOf(split, StringComparison.InvariantCulture);
                    if (splitIndex == Const.INDEX_NONE || splitIndex <= index)
                    {
                        allValid = false;
                        break;
                    }

                    index = splitIndex;
                }

                if (allValid)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion
    }

    internal enum LocalAssetFormat
    {
        Nothing,
        Byte,
        Text,
        Lines
    }
    
    public class LocalLoaderParameters : LoaderParameters
    {
        #region Fields
        internal LocalAssetFormat format = LocalAssetFormat.Nothing;
        #endregion
    }

    internal class LocalContentLoader : ContentLoader
    {
        private FileInfo fileInfo;
        private byte[] byteData;
        private string textData;
        private string[] linesData;

        public LocalContentLoader(string path, FileInfo fileInfo)
            : base(path)
        {
            this.fileInfo = fileInfo;
        }

        protected override void Load(LoaderParameters parameters)
        {
            status = ContentAsyncStatus.Loading;

            var tParameters = ValidateParameterType<LocalLoaderParameters>(parameters);
            if (!fileInfo.Exists)
            {
                status = ContentAsyncStatus.Failed;
                return;
            }

            status = ContentAsyncStatus.Failed;
            switch (tParameters.format)
            {
                case LocalAssetFormat.Byte:
                {
                    byteData = File.ReadAllBytes(fileInfo.FullName);
                    status = ContentAsyncStatus.Loaded;
                    break;
                }
                case LocalAssetFormat.Text:
                {
                    textData = File.ReadAllText(fileInfo.FullName);
                    status = ContentAsyncStatus.Loaded;
                    break;
                }
                case LocalAssetFormat.Lines:
                {
                    linesData = File.ReadAllLines(fileInfo.FullName);
                    status = ContentAsyncStatus.Loaded;
                    break;
                }
            }

            OnLoadCompleted();
        }

        protected override void Unload(LoaderParameters parameters)
        {
            byteData = null;
            textData = null;
            linesData = null;
        }
    }
}

namespace Prateek.Runtime.AppContentFramework.Local
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.AppContentFramework.Local.ContentFormats;
    using Prateek.Runtime.AppContentFramework.Local.Debug;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using UnityEngine;

    public class LocalRegistryServant
        : ContentRegistryServant
    {
        #region Static and Constants
        private const string EXTRA_CONTENT = "../ExtraContent";
        private const string EXTRA_CONTENT_TOC = "ContentLookUp.json";

        private const string SEARCH_PATTERN = "*.*";
        private const char WILDCARD = '*';

        private const string ERROR_FILE = "ErrorLog.txt";
        private static readonly string WILDCARD_S = $"{WILDCARD}";
        private static readonly char[] WILDCARD_A = {WILDCARD};
        #endregion

        #region Fields
        private bool addressSystemInitialized = false;

        private string extraContentPath = string.Empty;
        private string extraContentTocPath = string.Empty;
        private ContentToc contentToc = null;
        private List<ContentFormat> contentFormats = new List<ContentFormat>();
        private List<DirectoryInfo> lookUpInfos = new List<DirectoryInfo>();
        private Dictionary<string, ContentPath> pathToContentPaths = new Dictionary<string, ContentPath>();
        private List<ContentPath> cacheContentPaths = new List<ContentPath>();
        #endregion

        #region Properties
        public override bool IsAlive { get { return base.IsAlive && addressSystemInitialized; } }
        #endregion

        #region Class Methods
        public override void Startup()
        {
            base.Startup();

            foreach (var type in ContentFormatForagerWorker.Instance.FoundTypes)
            {
                contentFormats.Add(Activator.CreateInstance(type) as ContentFormat);
            }

            contentFormats.SortWithPriorities();
        }

        public override void ExecutingState(State state)
        {
            switch (state)
            {
                case State.Startup:
                {
                    extraContentPath = Path.Combine(Application.dataPath, EXTRA_CONTENT);
                    var contentInfo = new DirectoryInfo(extraContentPath);
                    if (!contentInfo.Exists)
                    {
                        break;
                    }

                    lookUpInfos.Add(contentInfo);

                    extraContentTocPath = Path.Combine(extraContentPath, EXTRA_CONTENT_TOC);

                    var builder        = (StringBuilder) null;
                    var contentTocInfo = new FileInfo(extraContentTocPath);
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
                        var errorPath = Path.Combine(extraContentPath, ERROR_FILE);
                        File.WriteAllText(errorPath, builder.ToString());
                    }

                    workStatus = lookUpInfos.Count > 0 ? WorkStatus.Pending : WorkStatus.Nothing;

                    break;
                }
                case State.StartWork:
                {
                    pathToContentPaths.Clear();
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

                            var relativePath = fileInfo.RelativePath(lookUpInfo);
                            if (pathToContentPaths.ContainsKey(relativePath))
                            {
                                pathToContentPaths.Remove(relativePath);
                            }

                            foreach (var contentFormat in contentFormats)
                            {
                                if (contentFormat.Extension != string.Empty && contentFormat.Extension.ToLower() != fileInfo.Extension.ToLower())
                                {
                                    continue;
                                }

                                cacheContentPaths.Clear();
                                if (contentFormat.ExtractPath(relativePath, fileInfo, cacheContentPaths))
                                {
                                    foreach (var contentPath in cacheContentPaths)
                                    {
                                        pathToContentPaths.Add(contentPath.StoragePath, contentPath);
                                    }
                                }

                                break;
                            }
                        }
                    }

                    foreach (var relativePath in pathToContentPaths.Keys)
                    {
                        ValidatePath(relativePath);
                    }

                    workStatus = WorkStatus.Done;

                    break;
                }
            }

            base.ExecutingState(state);
        }

        protected override ContentLoader GetNewContentLoader(string path)
        {
            return pathToContentPaths[path].ContentFormat.GetLoader(pathToContentPaths[path]);
        }

        public override void SetupDebugDocument(DebugMenuDocument document)
        {
            var section = new LocalRegistrySection(this, "Local Servant");

            document.AddSections(section);
        }

        private bool ShouldIgnore(FileInfo fileInfo)
        {
            if (contentToc == null)
            {
                return false;
            }

            if (fileInfo.Name == EXTRA_CONTENT_TOC)
            {
                return true;
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
                    splits = new[] {folder};
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

                    directoryInfo = directoryInfo.Parent;
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
                    splits = new[] {file};
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

        #region Nested type: ContentToc
        private class ContentToc
        {
            #region Fields
            public List<string> folders = new List<string>();
            public List<string> ignoredFolders = new List<string>();
            public List<string> ignoredFiles = new List<string>();
            #endregion
        }
        #endregion
    }
}

namespace Prateek.Editor.AtomicFileFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Editor.AtomicFileFramework.Detection;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Helpers;
    using UnityEditor;

    public class AtomicFileFormatter
    {
        #region Static and Constants
        public const string lineFeed = "{0}\r\n";

        private static SceneTracker sceneTracker = new SceneTracker();
        private static AtomicFileFormatter instance;
        #endregion

        #region Fields
        private List<AtomicFileFormat> fileFormats = new List<AtomicFileFormat>();
        private List<ImportInfo> pendingImports = new List<ImportInfo>();
        #endregion

        #region Properties
        public static AtomicFileFormatter Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AtomicFileFormatter();
                }

                return instance;
            }
        }
        #endregion

        #region Class Methods
        private void ClearPendingImports() { }

        internal void RegisterAll(List<Type> fileFormats)
        {
            foreach (var fileFormat in fileFormats)
            {
                var newFileFormat = Activator.CreateInstance(fileFormat) as AtomicFileFormat;
                if (newFileFormat == null)
                {
                    continue;
                }

                newFileFormat.Init();
                this.fileFormats.Add(newFileFormat);
            }
        }

        internal AtomicFileFormat GetFormat(string extension)
        {
            return fileFormats.Find(x => x.HeaderExtension == extension || x.PartExtension == extension);
        }

        internal void RequestImport(string assetPath)
        {
            var fileInfo = new FileInfo(assetPath);
            if (!fileInfo.Exists)
            {
                return;
            }

            var directoryInfo = fileInfo.Directory;
            if (pendingImports.FindIndex(x => x.directoryInfo.FullName == directoryInfo.FullName) != Const.INDEX_NONE)
            {
                return;
            }

            var fileFormat = GetFormat(fileInfo.Extension);
            if (fileFormat == null)
            {
                return;
            }

            pendingImports.Add(new ImportInfo
            {
                format = fileFormat,
                directoryInfo = directoryInfo
            });
        }

        internal void ProcessImport(string assetPath)
        {
            var fileInfo = new FileInfo(assetPath);
            if (!fileInfo.Exists)
            {
                return;
            }

            RequestImport(assetPath);

            var directoryInfo = fileInfo.Directory;
            var index         = pendingImports.FindIndex(x => x.directoryInfo.FullName == directoryInfo.FullName);
            if (index == Const.INDEX_NONE)
            {
                return;
            }

            ImportFile(pendingImports[index]);

            pendingImports.RemoveAt(index);
        }

        private static string GetExportFolder(FileInfo fileInfo, string contentExtension)
        {
            return Path.Combine(fileInfo.Directory.FullName, $"{Path.GetFileName(fileInfo.Name)}{contentExtension}");
        }

        private void ImportFile(ImportInfo importInfo)
        {
#if !USE_SCENE_SPLITTER
            return;
#endif

            var content = string.Empty;
            if (!importInfo.directoryInfo.Exists)
            {
                return;
            }

            content += LoadFile(importInfo.directoryInfo, importInfo.format.HeaderExtension);
            content += LoadFile(importInfo.directoryInfo, importInfo.format.PartExtension);

            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            var fileInfo = new FileInfo(importInfo.directoryInfo.FullName.RemoveExtension(importInfo.format.ContentExtension));
            File.WriteAllText(fileInfo.FullName, content);
        }

        internal void ExportFile(string path)
        {
#if !USE_SCENE_SPLITTER
            return;
#endif

            var fileInfo   = new FileInfo(path);
            var fileFormat = fileFormats.Find(x => x.OriginalExtension == fileInfo.Extension);
            if (fileFormat == null)
            {
                return;
            }

            var fileContent = File.ReadAllLines(fileInfo.FullName);

            var parts = new List<DetectedPart>();
            parts.Add(new DetectedPart
            {
                fileFormat = fileFormat,
                content = fileContent,
                startLine = 0,
                match = null
            });

            var detector = new PartDetector
            {
                fileFormat = fileFormat,
                content = fileContent,
                start = 0,
                parts = parts
            };

            detector.Detect();
            foreach (var part in parts)
            {
                part.LoadContent();
            }

            var destinationFolder = GetExportFolder(fileInfo, fileFormat.ContentExtension);
            var dirInfo           = new DirectoryInfo(destinationFolder);
            var headerFiles       = dirInfo.Exists ? dirInfo.GetFiles($"*{fileFormat.HeaderExtension}") : new FileInfo[0];
            var partFiles         = dirInfo.Exists ? dirInfo.GetFiles($"*{fileFormat.PartExtension}") : new FileInfo[0];

            var files = new HashSet<string>();
            foreach (var file in headerFiles)
            {
                files.Add(file.Name);
            }

            foreach (var file in partFiles)
            {
                files.Add(file.Name);
            }

            foreach (var part in parts)
            {
                var filename = part.GetFilename(fileInfo.Name);
                if (files.Contains(filename))
                {
                    files.Remove(filename);
                }

                part.Save(destinationFolder, filename);
            }

            foreach (var file in files)
            {
                File.Delete(Path.Combine(destinationFolder, file));
            }

            AssetDatabase.Refresh();
        }

        private static string LoadFile(DirectoryInfo directoryInfo, string extension)
        {
            var content = string.Empty;
            if (!directoryInfo.Exists)
            {
                return content;
            }

            var files = directoryInfo.GetFiles($"*{extension}");
            foreach (var file in files)
            {
                content += File.ReadAllText(file.FullName);
            }

            return content;
        }
        #endregion

        #region Nested type: ImportInfo
        private struct ImportInfo
        {
            public AtomicFileFormat format;
            public DirectoryInfo directoryInfo;
        }
        #endregion
    }
}

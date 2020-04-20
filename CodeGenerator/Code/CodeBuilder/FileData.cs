namespace Assets.Prateek.CodeGenerator.Code.CodeBuilder {
    using System;
    using System.IO;
    using global::Prateek.Core.Code.Helpers;
    using global::Prateek.Core.Code.Helpers.Files;

    public struct FileData
    {
        //-----------------------------------------------------------------
        public struct Infos
        {
            public FileInfo fileInfo;
            public DirectoryInfo directoryInfo;

            public string directory;
            public string name;
            public string extension;

            public void SetupFileInfo()
            {
                fileInfo = new FileInfo(Path.Combine(directory, name.Extension(extension)));
                directoryInfo = fileInfo.Directory;
            }

            public string content;
        }

        //-----------------------------------------------------------------
        public Infos source;
        public Infos destination;

        //-----------------------------------------------------------------
        public bool IsLoaded { get { return source.content != null && source.content != String.Empty; } }

        //-----------------------------------------------------------------
        public FileData(string file, string sourceDir) : this(file, sourceDir, null) { }
        public FileData(string file, string sourceDir, string content)
        {
            file = FileHelpers.GetValidFile(file);
            if ((content == null || content == String.Empty) && file == String.Empty)
            {
                source = default(Infos);
                destination = default(Infos);
                return;
            }

            source.fileInfo = null;
            source.directoryInfo = null;
            source.directory = Path.GetDirectoryName(file);
            source.name = PathPlus.GetFilenameOnly(file);
            source.extension = PathPlus.GetExtension(file);

            source.content = content;
                
            source.SetupFileInfo();

            destination = source;
        }


        //-----------------------------------------------------------------
        public bool Load(bool forceReload = false)
        {
            if (source.content != null)
            {
                if (!forceReload)
                    return true;
            }

            source.content = String.Empty;
            var path = source.fileInfo.Exists ? source.fileInfo.FullName : String.Empty;//FileHelpers.GetValidFile(source.absPath);
            if (path != String.Empty)
            {
                source.content = FileHelpers.ReadAllTextCleaned(path);
                destination.content = source.content;
                return true;
            }
            return false;
        }
    }
}
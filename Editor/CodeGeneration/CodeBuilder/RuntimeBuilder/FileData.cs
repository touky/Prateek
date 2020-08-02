namespace Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder
{
    using System;
    using System.IO;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.Core.Helpers.Files;

    public struct FileData
    {
        ///-----------------------------------------------------------------
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

        ///-----------------------------------------------------------------
        public Infos source;

        public Infos destination;

        ///-----------------------------------------------------------------
        public bool IsValid
        {
            get { return !string.IsNullOrEmpty(source.name); }
        }

        ///-----------------------------------------------------------------
        public bool IsLoaded
        {
            get { return !string.IsNullOrEmpty(source.content); }
        }

        ///-----------------------------------------------------------------
        public FileData(string file, string sourceDir) : this(file, sourceDir, null) { }

        public FileData(string file, string sourceDir, string content)
        {
            file = FileHelpers.GetValidFile(Path.Combine(sourceDir, file));
            if (string.IsNullOrEmpty(content) && string.IsNullOrEmpty(file))
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


        ///-----------------------------------------------------------------
        public bool Load(bool forceReload = false)
        {
            if (source.content != null)
            {
                if (!forceReload)
                    return true;
            }

            source.content = String.Empty;
            var path = source.fileInfo.Exists ? source.fileInfo.FullName : String.Empty; //FileHelpers.GetValidFile(source.absPath);
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

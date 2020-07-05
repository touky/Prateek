namespace Mayfair.Core.Editor.Database.ExcelToDatabaseContent
{
    using System;
    using System.IO;

    [Serializable]
    internal struct FileInfos
    {
        public string path;
        public long lastWriteTime;
        public FileInfosStatus rebuildStatus;

        public FileInfos(string path)
        {
            this.path = path;
            this.lastWriteTime = 0;
            this.rebuildStatus = FileInfosStatus.Nothing;

            MarkWriteTime();
        }

        public long GetLastWriteTime()
        {
            return File.GetLastWriteTime(this.path).ToFileTime();
        }

        public void MarkWriteTime()
        {
            this.lastWriteTime = GetLastWriteTime();
        }
    }
}
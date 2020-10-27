namespace Prateek.Runtime.AppContentFramework.Local
{
    using System.IO;
    using Prateek.Runtime.AppContentFramework.Local.ContentFormats;

    public class ContentPath
    {
        #region Fields
        private string storagePath;
        private FileInfo fileInfo;
        private FileInfo contentInfo;
        private ContentFormat contentFormat;
        #endregion

        #region Properties
        public string StoragePath { get { return storagePath; } }

        public FileInfo FileInfo { get { return fileInfo; } }

        public FileInfo ContentInfo { get { return contentInfo; } }

        public ContentFormat ContentFormat { get { return contentFormat; } }
        #endregion

        #region Constructors
        public ContentPath(string storagePath, FileInfo fileInfo, ContentFormat contentFormat)
        {
            this.storagePath = storagePath;
            this.fileInfo = fileInfo;
            this.contentFormat = contentFormat;
            contentInfo = fileInfo;
        }

        public ContentPath(string storagePath, FileInfo fileInfo, ContentFormat contentFormat, string contentPath)
            : this(storagePath, fileInfo, contentFormat)
        {
            contentInfo = new FileInfo(contentPath);
        }
        #endregion
    }
}

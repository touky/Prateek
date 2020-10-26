namespace Prateek.Runtime.AppContentFramework.Local.ContentLoader
{
    using System.IO;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Loader.Enums;
    using Prateek.Runtime.AppContentFramework.Local.Enums;

    internal class LocalContentLoader : ContentLoader
    {
        #region Fields
        private ContentPath contentPath;
        private byte[] byteData;
        private string textData;
        private string[] linesData;
        #endregion

        #region Constructors
        public LocalContentLoader(string path, ContentPath contentPath)
            : base(path)
        {
            this.contentPath = contentPath;
        }
        #endregion

        #region Class Methods
        protected override void Load(LoaderParameters parameters)
        {
            status = ContentAsyncStatus.Loading;

            var tParameters = ValidateParameterType<LocalLoaderParameters>(parameters);
            if (!contentPath.FileInfo.Exists)
            {
                status = ContentAsyncStatus.Failed;
                return;
            }

            status = ContentAsyncStatus.Failed;
            switch (tParameters.format)
            {
                case LocalAssetFormat.Byte:
                {
                    byteData = File.ReadAllBytes(contentPath.FileInfo.FullName);
                    status = ContentAsyncStatus.Loaded;
                    break;
                }
                case LocalAssetFormat.Text:
                {
                    textData = File.ReadAllText(contentPath.FileInfo.FullName);
                    status = ContentAsyncStatus.Loaded;
                    break;
                }
                case LocalAssetFormat.Lines:
                {
                    linesData = File.ReadAllLines(contentPath.FileInfo.FullName);
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
        #endregion
    }
}

namespace Prateek.Runtime.AppContentFramework.Local.ContentLoaders
{
    using System.IO;
    using global::Unity.Jobs;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;
    using Prateek.Runtime.AppContentFramework.Local.Enums;
    using UnityEngine.Assertions;

    internal class DefaultLocalContentLoader : LocalContentLoader
    {
        #region Fields
        private LocalLoaderParameters parameters;
        private byte[] byteData;
        private string textData;
        private string[] linesData;
        #endregion

        #region Constructors
        public DefaultLocalContentLoader(string path, ContentPath contentPath)
            : base(path, contentPath)
        {
        }
        #endregion

        #region Class Methods
        protected override LoadJob GetLoadJob(LoaderParameters parameters)
        {
            this.parameters = ValidateParameterType<LocalLoaderParameters>(parameters);
            switch (this.parameters.format)
            {
                case LocalAssetFormat.Byte:
                {
                    return new ReadAllBytes(contentPath.FileInfo.FullName);
                }
                case LocalAssetFormat.Text:
                {
                    return new ReadAllText(contentPath.FileInfo.FullName);
                }
                case LocalAssetFormat.Lines:
                {
                    return new ReadAllLines(contentPath.FileInfo.FullName);
                }
            }

            return null;
        }

        protected override void OnJobCompletion(LoadJob job)
        {
            switch (parameters.format)
            {
                case LocalAssetFormat.Byte:
                {
                    byteData = ((ReadAllBytes) job).ByteData;
                    break;
                }
                case LocalAssetFormat.Text:
                {
                    textData = ((ReadAllText) job).TextData;
                    break;
                }
                case LocalAssetFormat.Lines:
                {
                    linesData = ((ReadAllLines) job).LinesData;
                    break;
                }
            }
        }

        protected override void Unload(LoaderParameters parameters)
        {
            byteData = null;
            textData = null;
            linesData = null;
        }

        private class ReadAllBytes
            : LoadJob
        {
            private string path;
            private byte[] byteData;

            public byte[] ByteData => byteData;

            public ReadAllBytes(string path)
            {
                this.path = path;
                byteData = null;
            }
            
            protected override JobStatus ExecuteLoad()
            {
                byteData = File.ReadAllBytes(path);
                return JobStatus.Done;
            }
        }

        private class ReadAllText
            : LoadJob
        {
            private string path;
            private string textData;

            public string TextData => textData;

            public ReadAllText(string path)
            {
                this.path = path;
                textData = null;
            }

            protected override JobStatus ExecuteLoad()
            {
                textData = File.ReadAllText(path);
                return JobStatus.Done;
            }
        }

        private class ReadAllLines
            : LoadJob
        {
            private string path;
            private string[] linesData;

            public string[] LinesData => linesData;

            public ReadAllLines(string path)
            {
                this.path = path;
                linesData = null;
            }

            protected override JobStatus ExecuteLoad()
            {
                linesData = File.ReadAllLines(path);
                return JobStatus.Done;
            }
        }
        #endregion
    }
}

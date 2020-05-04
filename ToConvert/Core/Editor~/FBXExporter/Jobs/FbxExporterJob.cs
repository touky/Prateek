namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.Utils.Types.Priority;

    public abstract class FbxExporterJob : IPriority
    {
        #region Fields
        private readonly object jobLock = new object();
        private bool jobDone = false;
        protected string sourcePath = string.Empty;
        protected GUILogger logger;
        #endregion

        #region Properties
        public bool JobDone
        {
            get { return jobDone; }
            protected set
            {
                lock (jobLock)
                {
                    jobDone = value;
                }
            }
        }

        public string SourcePath
        {
            get { return sourcePath; }
        }

        public GUILogger Logger
        {
            set { logger = value; }
        }
        #endregion

        #region Constructors
        protected FbxExporterJob(string path)
        {
            sourcePath = path;
        }
        #endregion

        #region Class Methods
        protected void Log(string log)
        {
            if (logger == null)
            {
                return;
            }

            logger.Log(log);
        }

        public abstract void PreExecute();
        public abstract void Execute();
        public abstract void PostExecute();
        #endregion

        #region IPriority Members
        public abstract int Priority { get; }
        #endregion
    }
}

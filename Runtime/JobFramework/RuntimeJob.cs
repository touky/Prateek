namespace Prateek.Runtime.JobFramework
{
    public abstract class RuntimeJob
    {
        internal IMainThreadScheduler mainThreadScheduler;

        protected IMainThreadScheduler MainThreadScheduler { get { return mainThreadScheduler; } }

        #region Class Methods
        public abstract JobStatus Execute();

        protected void AddJob(MainThreadJob job)
        {
            mainThreadScheduler.AddJob(job);
        }
        #endregion

        public enum JobStatus
        {
            Working,
            Failed,
            Done,
        }
    }
}
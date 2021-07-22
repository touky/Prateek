namespace Prateek.Runtime.JobFramework
{
    public abstract class MainThreadJob
        : RuntimeJob
    {
        private object jobLock = new object();
        private JobStatus jobStatus = JobStatus.Working;

        public JobStatus JobStatus
        {
            get
            {
                lock (jobLock)
                {
                    return jobStatus;
                }
            }
            private set
            {
                lock (jobLock)
                {
                    jobStatus = value;
                }
            }
        }

        #region Class Methods
        internal JobStatus ExecuteOnMainThread()
        {
            var newStatus = Execute();
            JobStatus = newStatus;
            return newStatus;
        }
        #endregion
    }
}
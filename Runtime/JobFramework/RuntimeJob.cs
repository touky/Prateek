namespace Prateek.Runtime.JobFramework
{
    public abstract class RuntimeJob
    {
        #region Class Methods
        public abstract JobStatus Execute();
        #endregion

        public enum JobStatus
        {
            Failed,
            Working,
            Done,
        }
    }
}
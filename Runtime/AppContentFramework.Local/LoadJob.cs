namespace Prateek.Runtime.AppContentFramework.Local
{
    using System.Runtime.InteropServices.ComTypes;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;
    using Prateek.Runtime.JobFramework;

    public abstract class LoadJob
        : RuntimeJob
    {
        public ContentAsyncStatus Status { get; private set; }

        public override JobStatus Execute()
        {
            var status = ExecuteLoad();
            if (status == JobStatus.Working)
            {
                return JobStatus.Working;
            }

            if (status == JobStatus.Done)
            {
                Status = ContentAsyncStatus.Loaded;
                return JobStatus.Done;

            }

            Status = ContentAsyncStatus.Failed;
            return JobStatus.Failed;
        }

        protected abstract JobStatus ExecuteLoad();
    }
}
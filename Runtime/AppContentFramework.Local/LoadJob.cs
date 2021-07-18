namespace Prateek.Runtime.AppContentFramework.Local
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;
    using Prateek.Runtime.JobFramework;

    public abstract class LoadJob
        : RuntimeJob
    {
        public ContentAsyncStatus Status { get; private set; }

        public override bool Execute()
        {
            if (ExecuteLoad())
            {
                Status = ContentAsyncStatus.Loaded;
                return true;
            }

            Status = ContentAsyncStatus.Failed;
            return true;
        }

        protected abstract bool ExecuteLoad();
    }
}
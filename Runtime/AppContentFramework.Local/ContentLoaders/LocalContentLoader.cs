namespace Prateek.Runtime.AppContentFramework.Local.ContentLoaders
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;
    using Prateek.Runtime.AppContentFramework.Local.Interfaces;

    public abstract class LocalContentLoader : ContentLoader
    {
        private IJobHandler handler;

        protected ContentPath contentPath;

        protected LocalContentLoader(string path, ContentPath contentPath)
            : base(path)
        {
            this.contentPath = contentPath;
        }

        internal void Set(IJobHandler handler)
        {
            this.handler = handler;
        }

        protected override void Load(LoaderParameters parameters)
        {
            status = ContentAsyncStatus.Loading;

            if (!ValidateLoadOperation(parameters))
            {
                status = ContentAsyncStatus.Failed;

                OnLoadCompleted();
                return;
            }

            var job = GetLoadJob(parameters);
            if (job == null)
            {
                status = ContentAsyncStatus.Failed;
                return;
            }

            handler.Schedule(this, job);
        }

        protected virtual bool ValidateLoadOperation(LoaderParameters parameters)
        {
            return contentPath.FileInfo.Exists;
        }

        internal void JobCompleted(LoadJob loadJob)
        {
            status = loadJob.Status;

            OnJobCompletion(loadJob);

            OnLoadCompleted();
        }

        protected abstract LoadJob GetLoadJob(LoaderParameters parameters);
        protected abstract void OnJobCompletion(LoadJob job);
    }
}
namespace Prateek.Runtime.AppContentFramework.Local.Interfaces
{
    using Prateek.Runtime.AppContentFramework.Local.ContentLoaders;

    public interface IJobHandler
    {
        void Schedule(LocalContentLoader jobOwner, LoadJob job);
    }
}
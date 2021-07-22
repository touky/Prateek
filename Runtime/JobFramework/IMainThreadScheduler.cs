namespace Prateek.Runtime.JobFramework
{
    public interface IMainThreadScheduler
    {
        void AddJob(MainThreadJob job);
    }
}
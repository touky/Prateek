namespace Prateek.Runtime.JobFramework
{
    using UnityEngine;

    internal class MainThreadJobSystem
        : MonoBehaviour
            , IMainThreadScheduler
    {
        private JobQueue jobEntering = new JobQueue();
        private object jobLock = new object();

        public void AddJob(MainThreadJob job)
        {
            jobEntering.Enqueue(job);
        }

        private void Update()
        {
            var job = jobEntering.Dequeue() as MainThreadJob;
            if (job != null)
            {
                var jobStatus = job.ExecuteOnMainThread();
                if (jobStatus == RuntimeJob.JobStatus.Working)
                {
                    jobEntering.Enqueue(job);
                }
            }
        }
    }
}
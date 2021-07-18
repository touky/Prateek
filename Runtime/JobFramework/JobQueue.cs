namespace Prateek.Runtime.JobFramework
{
    using System.Collections.Generic;

    public class JobQueue
    {
        #region Static and Constants
        private Queue<RuntimeJob> jobQueue = new Queue<RuntimeJob>();
        private object jobLock = new object();
        #endregion

        #region Class Methods
        public void Enqueue(RuntimeJob job)
        {
            lock (jobLock)
            {
                jobQueue.Enqueue(job);
            }
        }

        public RuntimeJob Dequeue()
        {
            var newJob = (RuntimeJob) null;
            lock (jobLock)
            {
                if (jobQueue.Count > 0)
                {
                    newJob = jobQueue.Dequeue();
                }
            }

            return newJob;
        }

        public void Enqueue<TRuntimeJob>(Queue<TRuntimeJob> jobs)
            where TRuntimeJob : RuntimeJob
        {
            lock (jobLock)
            {
                while (jobs.Count > 0)
                {
                    jobQueue.Enqueue(jobs.Dequeue());
                }
            }
        }

        public void Dequeue<TRuntimeJob>(Queue<TRuntimeJob> jobs)
            where TRuntimeJob : RuntimeJob
        {
            lock (jobLock)
            {
                while (jobQueue.Count > 0)
                {
                    var job = jobQueue.Dequeue() as TRuntimeJob;
                    if (job == null)
                    {
                        continue;
                    }

                    jobs.Enqueue(job);
                }
            }
        }

        public void Dequeue<TRuntimeJob>(List<TRuntimeJob> jobs)
            where TRuntimeJob : RuntimeJob
        {
            lock (jobLock)
            {
                while (jobQueue.Count > 0)
                {
                    var job = jobQueue.Dequeue() as TRuntimeJob;
                    if (job == null)
                    {
                        continue;
                    }

                    jobs.Add(job);
                }
            }
        }
        #endregion
    }
}

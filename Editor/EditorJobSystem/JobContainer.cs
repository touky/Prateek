namespace Prateek.Editor.EditorJobSystem
{
    using System.Collections.Generic;

    internal class JobContainer
    {
        #region Static and Constants
        private Queue<ThreadedJob> jobQueue = new Queue<ThreadedJob>();
        private object jobLock = new object();
        #endregion

        #region Class Methods
        public void Enqueue(ThreadedJob job)
        {
            lock (jobLock)
            {
                jobQueue.Enqueue(job);
            }
        }

        public ThreadedJob Dequeue()
        {
            var newJob = (ThreadedJob) null;
            lock (jobLock)
            {
                if (jobQueue.Count > 0)
                {
                    newJob = jobQueue.Dequeue();
                }
            }

            return newJob;
        }

        public void Enqueue(Queue<ThreadedJob> jobs)
        {
            lock (jobLock)
            {
                while (jobs.Count > 0)
                {
                    jobQueue.Enqueue(jobs.Dequeue());
                }
            }
        }

        public void Dequeue(Queue<ThreadedJob> jobs)
        {
            lock (jobLock)
            {
                while (jobQueue.Count > 0)
                {
                    jobs.Enqueue(jobQueue.Dequeue());
                }
            }
        }
        #endregion
    }
}

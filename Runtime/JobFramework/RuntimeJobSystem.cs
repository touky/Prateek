namespace Prateek.Runtime.JobFramework
{
    using System.Collections.Generic;
    using System.Threading;

    public class RuntimeJobSystem
    {
        #region Static and Constants
        private const int exitMainFrameCount = 100;
        private const int waitMilliseconds = 100;

        private bool emergencyExit = true;

        private Thread mainThread = null;

        private JobQueue jobEntering = new JobQueue();
        private JobQueue jobFinished = new JobQueue();
        #endregion

        #region Class Methods
        public void AddWork(RuntimeJob job)
        {
            if (mainThread == null || !mainThread.IsAlive)
            {
                emergencyExit = false;

                mainThread = new Thread(WorkThread);
                mainThread.Name = $"RuntimeJobSystem.ThreadUpdate";
                mainThread.Start();
            }

            jobEntering.Enqueue(job);
        }

        public void GetFinishedWork<TRuntimeJob>(List<TRuntimeJob> resultJobs)
            where TRuntimeJob : RuntimeJob
        {
            jobFinished.Dequeue(resultJobs);
        }

        private void WorkThread()
        {
            var frameCount = exitMainFrameCount;
            while (true)
            {
                var workJobs = new Queue<RuntimeJob>();
                var workingJobs = new Queue<RuntimeJob>();
                var doneJobs = new Queue<RuntimeJob>();
                jobEntering.Dequeue(workJobs);

                bool resetFrameCount = false;
                while (workJobs.Count > 0)
                {
                    resetFrameCount = true;

                    var job = workJobs.Dequeue();
                    var jobStatus = job.Execute();
                    if (jobStatus != RuntimeJob.JobStatus.Working)
                    {
                        doneJobs.Enqueue(job);
                    }
                    else
                    {
                        workingJobs.Enqueue(job);
                    }
                }

                jobEntering.Enqueue(workingJobs);
                jobFinished.Enqueue(doneJobs);

                if (resetFrameCount)
                {
                    frameCount = exitMainFrameCount;
                }
                else
                {
                    frameCount--;
                }

                if (frameCount < 0 || emergencyExit)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(waitMilliseconds);
                }
            }
        }
        #endregion
    }
}
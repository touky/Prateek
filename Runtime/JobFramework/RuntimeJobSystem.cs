namespace Prateek.Runtime.JobFramework
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Prateek.Runtime.Core.Singleton;
    using UnityEngine;

    public class RuntimeJobSystem
    {
        #region Static and Constants
        private const int exitMainFrameCount = 100;
        private const int waitMilliseconds = 100;

        private bool emergencyExit = true;

        private Thread thread = null;
        private MainThreadJobSystem mainThreadScheduler;

        private JobQueue jobEntering = new JobQueue();
        private JobQueue jobFinished = new JobQueue();
        #endregion

        #region Class Methods
        public void AddWork(RuntimeJob job)
        {
            StartThread();
            StartMainThreadScheduler();

            job.mainThreadScheduler = mainThreadScheduler;

            jobEntering.Enqueue(job);
        }

        public void GetFinishedWork<TRuntimeJob>(List<TRuntimeJob> resultJobs)
            where TRuntimeJob : RuntimeJob
        {
            jobFinished.Dequeue(resultJobs);
        }

        private void StartThread()
        {
            if (thread == null || !thread.IsAlive)
            {
                emergencyExit = false;

                thread = new Thread(WorkThread);
                thread.Name = $"RuntimeJobSystem.ThreadUpdate";
                thread.Start();
            }
        }

        private void StartMainThreadScheduler()
        {
            if (mainThreadScheduler != null)
            {
                return;
            }

            mainThreadScheduler = new GameObject($"{nameof(MainThreadJobSystem)}").AddComponent<MainThreadJobSystem>();
            ParentProvider.AddChildToParent(mainThreadScheduler.transform, "MultiThreading-Coroutine");
            GameObject.DontDestroyOnLoad(mainThreadScheduler);
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
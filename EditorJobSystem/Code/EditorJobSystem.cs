namespace Assets.Prateek.EditorJobSystem
{
    using System.Collections.Generic;
    using System.Threading;
    using UnityEngine;

    public static class EditorJobSystem
    {
        #region Static and Constants
        private const int exitMainFrameCount = 10;
        private const int exitChildFrameCount = 100;
        private const int waitMilliseconds = 100;
        private const int workThreadCount = 5;

        private static bool emergencyExit = true;

        private static Thread mainThread = null;
        private static object inOrderLock = new object();
        private static Thread inOrderThread = null;
        private static object outOfOrderLock = new object();
        private static List<Thread> outOfOrderThreads = new List<Thread>();

        private static JobContainer jobEntering = new JobContainer();
        private static JobContainer jobInOrder = new JobContainer();
        private static JobContainer jobOutOfOrder = new JobContainer();
        private static JobContainer jobFinished = new JobContainer();
        private static JobContainer jobExiting = new JobContainer();
        #endregion

        #region Class Methods
        public static void AddWork(ThreadedJob job)
        {
            if (mainThread == null)
            {
                emergencyExit = false;

                mainThread = new Thread(DispatchThread);
                mainThread.Name = "TemplateRegistry.ThreadUpdate";
                mainThread.Start();
            }


            jobEntering.Enqueue(job);
        }

        private static void StartInOrderThread()
        {
            lock (inOrderLock)
            {
                if (inOrderThread == null || !inOrderThread.IsAlive)
                {
                    var thread = new Thread(WorkThreadInOrder);

                    thread.Name = "TemplateRegistry.WorkThreadInOrder";
                    thread.Start();

                    inOrderThread = thread;
                }
            }
        }

        private static void StartOutOfOrderThreads()
        {
            lock (outOfOrderLock)
            {
                UnsafeCleanOutOfOrderThreads();

                int threadAdded = Mathf.Max(0, Mathf.Min(workThreadCount - outOfOrderThreads.Count, workThreadCount));
                if (threadAdded == 0)
                {
                    return;
                }

                for (var t = 0; t < threadAdded; t++)
                {
                    var thread = new Thread(WorkThreadOutOfOrder);

                    thread.Name = "TemplateRegistry.WorkThreadOutOfOrder";
                    thread.Start();

                    outOfOrderThreads.Add(thread);
                }
            }
        }

        private static void UnsafeCleanOutOfOrderThreads()
        {
            outOfOrderThreads.RemoveAll(x => { return x == null || x.IsAlive; });
        }

        public static void JoinWork(Queue<ThreadedJob> resultJobs = null)
        {
            if (mainThread == null)
            {
                return;
            }

            mainThread.Join();

            if (resultJobs != null)
            {
                jobExiting.Enqueue(resultJobs);
            }
        }

        private static void DispatchThread()
        {
            var frameCount = exitMainFrameCount;
            while (true)
            {
                var tempJobs = new Queue<ThreadedJob>();
                jobEntering.Dequeue(tempJobs);

                var inOrderJobs    = new Queue<ThreadedJob>();
                var outOfOrderJobs = new Queue<ThreadedJob>();
                while (tempJobs.Count > 0)
                {
                    var job = tempJobs.Dequeue();
                    if (job.DispatchInOrder)
                    {
                        inOrderJobs.Enqueue(job);
                    }
                    else
                    {
                        outOfOrderJobs.Enqueue(job);
                    }
                }

                if (inOrderJobs.Count > 0)
                {
                    StartInOrderThread();
                }

                if (outOfOrderJobs.Count > 0)
                {
                    StartOutOfOrderThreads();
                }

                jobInOrder.Enqueue(inOrderJobs);
                jobOutOfOrder.Enqueue(outOfOrderJobs);

                jobFinished.Dequeue(tempJobs);
                jobExiting.Enqueue(tempJobs);

                bool resetFrameCount = false;
                lock (inOrderLock)
                {
                    if (inOrderThread == null || !inOrderThread.IsAlive)
                    {
                        resetFrameCount = true;
                    }
                }

                lock (outOfOrderLock)
                {
                    if (outOfOrderThreads.Count > 0)
                    {
                        UnsafeCleanOutOfOrderThreads();
                        resetFrameCount = true;
                    }
                }

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

            mainThread = null;
        }

        private static void WorkThreadInOrder()
        {
            var frameCount = exitChildFrameCount;
            while (true)
            {
                var newJob = jobInOrder.Dequeue();
                if (newJob != null)
                {
                    if (newJob.Execute())
                    {
                        jobFinished.Enqueue(newJob);
                    }

                    frameCount = exitChildFrameCount;
                }
                else
                {
                    frameCount--;
                }

                if (frameCount < 0)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(waitMilliseconds);
                }
            }
        }

        private static void WorkThreadOutOfOrder()
        {
            var frameCount = exitChildFrameCount;
            while (true)
            {
                var newJob = jobOutOfOrder.Dequeue();
                if (newJob != null)
                {
                    if (newJob.Execute())
                    {
                        jobFinished.Enqueue(newJob);
                    }

                    frameCount = exitChildFrameCount;
                }
                else
                {
                    frameCount--;
                }

                if (frameCount < 0)
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

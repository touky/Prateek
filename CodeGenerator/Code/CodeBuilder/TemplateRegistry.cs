namespace Prateek.CodeGenerator {
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using Prateek.CodeGenerator.ScriptTemplates;
    using UnityEngine;

    public static class TemplateRegistry
    {
        //---------------------------------------------------------------------
        #region Scripts
        private static List<ScriptFileTemplate> scripts = new List<ScriptFileTemplate>();
        private static System.Object scriptsLock = new object();

        public static TemplateGroup<ScriptFileTemplate> Scripts
        {
            get
            {
                WaitForLoad();
                return new TemplateGroup<ScriptFileTemplate>(scripts);
            }
        }

        public static void Add(ScriptFileTemplate data)
        {
            lock (scriptsLock)
            {
                scripts.Add(data);
            }
        }
        #endregion Scripts

        //---------------------------------------------------------------------
        #region Keywords
        private static List<KeywordTemplate> keywords = new List<KeywordTemplate>();
        private static System.Object keywordsLock = new object();

        public static TemplateGroup<KeywordTemplate> Keywords
        {
            get
            {
                WaitForLoad();
                return new TemplateGroup<KeywordTemplate>(keywords);
            }
        }

        public static void Add(KeywordTemplate data)
        {
            lock (keywordsLock)
            {
                keywords.Add(data);
            }
        }
        #endregion Keywords

        //---------------------------------------------------------------------
        #region Ignorables
        private static List<IgnorableTemplate> ignorables = new List<IgnorableTemplate>();
        private static System.Object ignorablesLock = new object();

        public static TemplateGroup<IgnorableTemplate> Ignorables
        {
            get
            {
                WaitForLoad();
                return new TemplateGroup<IgnorableTemplate>(ignorables);
            }
        }

        public static void Add(IgnorableTemplate data)
        {
            lock (keywordsLock)
            {
                ignorables.Add(data);
            }
        }
        #endregion Ignorables

        //---------------------------------------------------------------------
        #region Unity templates
        private static List<UnityFileTemplate> templates = new List<UnityFileTemplate>();
        private static System.Object templatesLock = new object();

        public static void Add(UnityFileTemplate data)
        {
            lock (templatesLock)
            {
                templates.Add(data);
            }
        }

        public static bool MatchTemplate(string filePath, string extension, string content)
        {
            WaitForLoad();
            for (int t = 0; t < templates.Count; t++)
            {
                var template = templates[t];
                if (template.FullName != filePath)
                    continue;

                return template.Match(template.FileName, extension, content);
            }
            return false;
        }

        private static System.Object jobLock = new object();
        private static Thread mainThread = null;
        private static Queue<LoadJob> jobQueue = new Queue<LoadJob>();
        public static void Add(LoadJob job)
        {
            if (mainThread == null)
            {
                mainThread = new Thread(ThreadUpdate);
                mainThread.Name = "TemplateRegistry.ThreadUpdate";
                mainThread.Start();
            }

            lock (jobLock)
            {
                jobQueue.Enqueue(job);
            }
        }

        private static void WaitForLoad()
        {
            if (mainThread == null)
            {
                return;
            }

            mainThread.Join();
        }

        private static void ThreadUpdate()
        {
            int loadedFiles = 0;
            int exitFrameCount = 10;
            while (true)
            {
                var newJob = (LoadJob) null;
                lock (jobLock)
                {
                    if (jobQueue.Count > 0)
                    {
                        newJob = jobQueue.Dequeue();
                    }
                }

                if (newJob != null)
                {
                    newJob.template.LoadFile(newJob.contentPath).Commit();
                    loadedFiles++;

                    exitFrameCount = 10;
                }

                exitFrameCount--;
                if (exitFrameCount < 0)
                {
                    break;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }

            mainThread = null;
        }
        #endregion Unity templates
    }

    public class LoadJob
    {
        public string contentPath;
        public BaseTemplate template;

        public void Commit()
        {
            TemplateRegistry.Add(this);
        }
    }
}

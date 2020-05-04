namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    using System.Collections.Generic;
    using Code.Utils;
    using Mayfair.Core.Editor.AssetLibrary;
    using UnityEditor;
    using UnityEngine;
    using Utils;

    public class FbxExporterFileExportJob : FbxExporterMatchJob
    {
        #region JobState enum
        private enum JobState
        {
            DestinationCheckout,
            Export,
            Done
        }
        #endregion

        #region Static and Constants
        public const int DEFAULT_PRIORITY = 20;
        #endregion

        #region Fields
        private readonly object contentLock = new object();
        private int priority = DEFAULT_PRIORITY;
        private JobState jobState = JobState.DestinationCheckout;
        private List<ExportContent> exportedContents = new List<ExportContent>();
        protected FbxExporterReimportJob reimportJob;
        #endregion

        #region Properties
        public override int Priority
        {
            get { return AssetLibraryConsts.PROCESSING_BEGIN + priority; }
        }
        #endregion

        #region Constructors
        public FbxExporterFileExportJob(string path, FbxExporterReimportJob reimportJob, int priority = DEFAULT_PRIORITY) : base(path)
        {
            this.priority = priority;
            this.reimportJob = reimportJob;
        }
        #endregion

        #region Class Methods
        public override void Execute()
        {
            if (!JobDone)
            {
                switch (jobState)
                {
                    case JobState.DestinationCheckout:
                    {
                        Log("Checkout destinations files");
                        foreach (ExportContent content in exportedContents)
                        {
                            string result = ExporterHelper.CheckoutDestination(content);
                            Log($"    Checking out {result} ...");
                        }

                        jobState = JobState.Export;
                        break;
                    }
                    case JobState.Export:
                    {
                        Log("Export files in destination");
                        foreach (ExportContent content in exportedContents)
                        {
                            Log($"    Exporting {content.FileName} ...");
                        }

                        ExporterHelper.ExportToFile(exportedContents);

                        foreach (ExportContent content in exportedContents)
                        {
                            if (content.ContentCount == 0)
                            {
                                continue;
                            }

                            reimportJob.Files.Add(content.ExportPath);

                            if (content.ContentAtIndex(Consts.FIRST_ITEM) is Object objectContent)
                            {
                                Object.DestroyImmediate(objectContent);
                            }
                        }

                        jobState = JobState.Done;
                        break;
                    }
                }
            }

            JobDone = jobState == JobState.Done;
        }

        public override void PostExecute()
        {
            AssetDatabase.SaveAssets();
            foreach (ExportContent content in exportedContents)
            {
                AssetDatabase.ImportAsset(content.ExportPath, ImportAssetOptions.ForceUpdate);
            }
        }

        public void AddContent(List<ExportContent> contents)
        {
            lock (contentLock)
            {
                this.exportedContents.AddRange(contents);
            }
        }
        #endregion
    }
}

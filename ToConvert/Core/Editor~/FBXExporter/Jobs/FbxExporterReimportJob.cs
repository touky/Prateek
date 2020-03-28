namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    using Mayfair.Core.Editor.AssetLibrary;
    using System.Collections.Generic;
    using UnityEditor;

    public class FbxExporterReimportJob : FbxExporterJob
    {
        #region Static and Constants
        public const int DEFAULT_PRIORITY = FbxExporterFileExportJob.DEFAULT_PRIORITY + 10;
        #endregion

        #region Fields
        private int priority = DEFAULT_PRIORITY;
        private List<string> files = new List<string>();
        #endregion

        #region Properties
        public override int Priority
        {
            get { return AssetLibraryConsts.PROCESSING_BEGIN + priority; }
        }

        public List<string> Files
        {
            get { return files; }
        }
        #endregion

        #region Constructors
        public FbxExporterReimportJob(string path, int priority = DEFAULT_PRIORITY) : base(path)
        {
            this.priority = priority;
        }
        #endregion

        #region Class Methods
        public override void PreExecute() { }

        public override void Execute()
        {
            AssetDatabase.SaveAssets();

            foreach (string file in files)
            {
                Log($"Reimporting {file}");
                AssetDatabase.ImportAsset(file, ImportAssetOptions.ForceUpdate);
            }

            JobDone = true;
        }

        public override void PostExecute() { }
        #endregion
    }
}

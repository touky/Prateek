namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    using System.Collections.Generic;
    using Mayfair.Core.Editor.AssetLibrary;

    public abstract class FbxExporterPackageCreationJob : FbxExporterMatchJob
    {
        #region Fields
        protected List<ExportContent> exportedContents = new List<ExportContent>();
        protected FbxExporterFileExportJob exportJob;
        #endregion

        #region Properties
        public override int Priority
        {
            get { return AssetLibraryConsts.PROCESSING_BEGIN; }
        }

        public List<ExportContent> ExportedContents
        {
            get { return exportedContents; }
        }
        #endregion

        #region Constructors
        public FbxExporterPackageCreationJob(string path, FbxExporterFileExportJob exportJob) : base(path)
        {
            this.exportJob = exportJob;
        }
        #endregion

        #region Class Methods
        public override void Execute() { JobDone = true; }

        public override void PostExecute()
        {
        }
        #endregion
    }
}

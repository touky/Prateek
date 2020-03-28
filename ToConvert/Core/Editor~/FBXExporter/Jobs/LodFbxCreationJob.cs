namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    public class LodFbxCreationJob : FbxExporterPackageCreationJob
    {
        #region Constructors
        public LodFbxCreationJob(string path, FbxExporterFileExportJob exportJob) : base(path, exportJob) { }
        #endregion

        #region Class Methods
        public override void Execute()
        {
            if (!JobDone)
            {
                FbxExportHelper.CreateLodContent(instances, exportedContents);
                exportJob.AddContent(exportedContents);
            }

            base.Execute();
        }
        #endregion
    }
}

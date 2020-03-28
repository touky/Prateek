namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    public class UniqueFbxCreationJob : FbxExporterPackageCreationJob
    {
        #region Constructors
        public UniqueFbxCreationJob(string path, FbxExporterFileExportJob exportJob) : base(path, exportJob) { }
        #endregion

        #region Class Methods
        public override void Execute()
        {
            if (!JobDone)
            {
                FbxExportHelper.CreateUniqueContent(instances, exportedContents);
                exportJob.AddContent(exportedContents);
            }

            JobDone = true;
        }
        #endregion
    }
}

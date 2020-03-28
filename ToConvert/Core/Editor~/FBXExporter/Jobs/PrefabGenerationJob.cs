namespace Mayfair.Core.Editor.FBXExporter.Jobs
{
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.VisualAsset;
    using UnityEngine;

    public class PrefabGenerationJob : FbxExporterPackageCreationJob
    {
        #region Static and Constants
        public const int DEFAULT_PRIORITY = 40;
        #endregion

        #region Properties
        public override int Priority
        {
            get { return AssetLibraryConsts.PROCESSING_BEGIN + DEFAULT_PRIORITY; }
        }
        #endregion

        #region Constructors
        public PrefabGenerationJob(string path, FbxExporterFileExportJob exportJob) : base(path, exportJob) { }
        #endregion

        #region Class Methods
        public override void Execute()
        {
            GameObject prefab = VisualAssetBuilderHelper.GenerateVisualAssetPrefab(instances[0].originalTransform.gameObject, sourcePath, logger);
            List<GameObject> content = new List<GameObject>();
            Transform[] transforms = prefab.GetComponentsInChildren<Transform>();
            content.AddUnique(prefab);

            foreach (Transform transform in transforms)
            {
                content.AddUnique(transform.gameObject);
            }

            PrefabExportContent exportContent = new PrefabExportContent()
            {
                OriginalPath = sourcePath,
                FileName = Path.GetFileNameWithoutExtension(sourcePath),
                ExportedContent = content
            };
            
            exportedContents.Add(exportContent);
            Log($"Generating prefab: {exportContent.FileName}");
            exportJob.AddContent(exportedContents);

            base.Execute();
        }
        #endregion
    }
}

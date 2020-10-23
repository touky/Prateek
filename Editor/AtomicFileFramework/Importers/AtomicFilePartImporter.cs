namespace Prateek.Editor.AtomicFileFramework.Importers
{
    using System.IO;
    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;
    using UnityEngine;

    [ScriptedImporter(5, AtomicFileFormat.PART_EXTENSION, IMPORT_ORDER)]
    public class AtomicFilePartImporter : ScriptedImporter
    {
        #region Static and Constants
        internal const int IMPORT_ORDER = -10;
        #endregion

        #region Class Methods
        public override void OnImportAsset(AssetImportContext ctx)
        {
            var assetInfo  = new FileInfo(ctx.assetPath);
            var fileFormat = AtomicFileFormatter.Instance.GetFormat(assetInfo.Extension);
            if (fileFormat == null)
            {
                return;
            }

            var headerInfo = fileFormat.GetAtomicHeader(assetInfo);
            if (!headerInfo.Exists)
            {
                Debug.LogError($"Atomic part file '{assetInfo.Name}' could not find its header counterpart '{headerInfo.Name}'", ctx.mainObject);
                return;
            }

            AssetDatabase.ImportAsset(headerInfo.FullName, ImportAssetOptions.ForceUpdate);
        }
        #endregion
    }
}

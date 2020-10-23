namespace Prateek.Editor.AtomicFileFramework.Importers
{
    using System.IO;
    using Prateek.Runtime.Core.Consts;
    using UnityEditor;
    using UnityEditor.Experimental.AssetImporters;

    [ScriptedImporter(3, AtomicFileFormat.HEADER_EXTENSION, AtomicFilePartImporter.IMPORT_ORDER + Const.NEXT_ITEM, AllowCaching = false)]
    public class AtomicFileHeaderImporter : ScriptedImporter
    {
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
            if (headerInfo.Name != assetInfo.Name)
            {
                AssetDatabase.ImportAsset(headerInfo.FullName, ImportAssetOptions.ForceUpdate);
                return;
            }

            AtomicFileFormatter.Instance.ProcessImport(assetPath);
        }
        #endregion
    }
}

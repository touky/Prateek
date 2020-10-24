namespace Prateek.Editor.AtomicFileFramework
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;

    internal class AtomicFilePostprocessor : AssetPostprocessor
    {
        #region Class Methods
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            var imported = new HashSet<string>();
            var pending  = new HashSet<string>();

            AnalyzeImports(importedAssets, imported, pending);
            AnalyzeImports(movedAssets, imported, pending);

            foreach (var assetPath in pending)
            {
                AtomicFileFormatter.Instance.ProcessImport(assetPath);
            }
        }

        private static void AnalyzeImports(string[] assets, HashSet<string> imported, HashSet<string> pending)
        {
            foreach (var assetPath in assets)
            {
                var assetInfo = new FileInfo(assetPath);
                var fileFormat = AtomicFileFormatter.Instance.GetFormat(assetInfo.Extension);
                if (fileFormat == null)
                {
                    continue;
                }

                var headerInfo = fileFormat.GetAtomicHeader(assetInfo);
                if (assetInfo.FullName == headerInfo.FullName)
                {
                    imported.Add(headerInfo.FullName);
                    pending.Remove(headerInfo.FullName);
                }
                else if (!pending.Contains(assetInfo.FullName) && !imported.Contains(assetInfo.FullName))
                {
                    pending.Add(headerInfo.FullName);
                }
            }
        }
        #endregion
    }
}

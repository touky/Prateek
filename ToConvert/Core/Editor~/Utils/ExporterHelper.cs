namespace Mayfair.Core.Editor.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Code.Utils;
    using Code.Utils.Helpers.Regexp;
    using FBXExporter;
    using UnityEditor;
    using UnityEditor.Formats.Fbx.Exporter;
    using UnityEngine;

    public static class ExporterHelper
    {
        #region Class Methods
        public static bool ExportToFile(List<ExportContent> exportedContents)
        {
            bool fullSuccess = true;
            for (int c = 0; c < exportedContents.Count; c++)
            {
                ExportContent content = exportedContents[c];
                if (!ExportToFile(content))
                {
                    fullSuccess = false;
                }

                exportedContents[c] = content;
            }

            return fullSuccess;
        }

        public static string CheckoutDestination(ExportContent content)
        {
            string exportPath = GetExportPath(content);
            SourceControlHelper.CheckoutOrAdd(exportPath);
            return exportPath;
        }

        public static bool ExportToFile(ExportContent content)
        {
            content.Success = false;

            string exportPath = content.GetExportPath();

            //Do not export directly into the correct folder, first into the Library/
            //This is necessary to prevent unity from registering the Delete/Add as a pure Delete half of the time
            string tempPath = AssetEditHelper.GetTemporaryFolder(exportPath);
            
            string result = content.ExportData(tempPath);
            if (result == null)
            {
                Debug.LogError($"Export failed {Path.GetFileName(exportPath)}");
            }
            else
            {
                SourceControlHelper.CheckoutOrAdd(exportPath);

                //Move asset out of the Library/
                try
                {
                    if (IOHelper.MoveOrReplaceFile(tempPath, exportPath))
                    {
                        SourceControlHelper.CheckoutOrAdd(exportPath);
                        content.ExportPath = exportPath;
                        content.Success = true;
                    }
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                    return false;
                }

                return true;
            }

            return false;
        }
       
        public static string GetExporterExtension(ExporterType type)
        {
            switch (type)
            {
                case ExporterType.FBX:    { return ConstsEditor.FBX; }
                case ExporterType.PREFAB: { return ConstsEditor.PREFAB; }
            }

            return null;
        }

        public static string GetExportAssetPath(string assetPath, string newName)
        {
            string replacedFolder = PathRegex.DetectFolderAfterRoot(ConstsFolders.ART_ROOT, assetPath);
            if (string.IsNullOrEmpty(replacedFolder))
            {
                return string.Empty;
            }

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            assetPath = assetPath.Replace($"/{replacedFolder}/", $"/{ConstsFolders.ART_EXPORT_ROOT}/");
            assetPath = assetPath.Replace($"/{fileName}.", $"/{newName}.");
            assetPath = PathHelper.Simplify(assetPath);

            return assetPath;
        }

        private static string GetExportPath(ExportContent content)
        {
            string exportPath = content.OriginalPath;
            string newName = content.FileName;

            exportPath = GetExportAssetPath(exportPath, newName);

            string dstExtension = GetExporterExtension(content.Type);
            {
                string srcExtension = Path.GetExtension(exportPath);
                exportPath = exportPath.Replace($"{srcExtension}", $".{dstExtension}");
            }

            return exportPath;
        }
        #endregion
    }
}

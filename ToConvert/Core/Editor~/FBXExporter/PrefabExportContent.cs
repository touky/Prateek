namespace Mayfair.Core.Editor.FBXExporter
{
    using System.Collections.Generic;
    using System.IO;
    using Code.Utils;
    using Code.Utils.Helpers.Regexp;
    using UnityEditor;
    using UnityEngine;
    using Utils;

    public class PrefabExportContent : ExportContent
    {
        public List<GameObject> ExportedContent { get; set; } = new List<GameObject>();

        public override ExporterType Type => ExporterType.PREFAB;

        public override int ContentCount => ExportedContent.Count;

        public override string ExportData(string path)
        {
            if (ExportedContent.Count == 0)
            {
                Success = false;
                return string.Empty;
            }
            
            PrefabUtility.SaveAsPrefabAsset(ExportedContent[0], path, out bool succeed);
            Success = succeed;
            return Success ? path : string.Empty;
        }

        public override string GetExportPath()
        {
            string path = OriginalPath;
            string newName = FileName;

            path = GetArtExportAssetPath(path, newName);

            string dstExtension = ConstsEditor.PREFAB;
            {
                string srcExtension = Path.GetExtension(path);
                path = path.Replace($"{srcExtension}", $".{dstExtension}");
            }

            return path;
        }

        public override object ContentAtIndex(int index)
        {
            return ExportedContent[index];
        }
    }
}
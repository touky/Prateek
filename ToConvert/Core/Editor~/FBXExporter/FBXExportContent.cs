namespace Mayfair.Core.Editor.FBXExporter
{
    using System.Collections.Generic;
    using System.IO;
    using Code.Utils;
    using Code.Utils.Helpers.Regexp;
    using UnityEditor.Formats.Fbx.Exporter;
    using UnityEngine;
    using Utils;

    public class FBXExportContent : ExportContent
    {
        public List<GameObject> ExportedContent { get; set; } = new List<GameObject>();

        public override ExporterType Type => ExporterType.FBX;

        public override int ContentCount => ExportedContent.Count;

        public override string ExportData(string path)
        {
            //Fix for 3ds, everything needs to have a different name, or an "_n" suffix will be added at the end
            int count = 0;
            HashSet<string> names = new HashSet<string>();
            foreach (GameObject gameObject in ExportedContent)
            {
                if (names.Contains(gameObject.name))
                {
                    gameObject.name = $"{gameObject.name}{++count}";
                }
                else
                {
                    names.Add(gameObject.name);
                }
            }

            return ModelExporter.ExportObjects(path, ExportedContent.ToArray());
        }

        public override string GetExportPath()
        {
            string path = OriginalPath;
            string newName = FileName;

            path = GetArtExportAssetPath(path, newName);

            string dstExtension = ConstsEditor.FBX;
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

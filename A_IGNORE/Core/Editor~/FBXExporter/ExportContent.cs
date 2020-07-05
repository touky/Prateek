namespace Mayfair.Core.Editor.FBXExporter
{
    using System.Collections.Generic;
    using System.IO;
    using Code.Utils;
    using Code.Utils.Helpers.Regexp;
    using Utils;

    public abstract class ExportContent
    {
        public bool Success { get; set; }
        public string OriginalPath { get; set; }
        public string FileName { get; set; }
        public string ExportPath { get; set; }

        public abstract ExporterType Type { get; }
        public abstract int ContentCount { get; }
        
        public abstract string ExportData(string path);
        public abstract string GetExportPath();
        public abstract object ContentAtIndex(int index);

        protected static string GetArtExportAssetPath(string assetPath, string newName)
        {
            string replacedFolder = PathRegex.DetectFolderAfterRoot(ConstsFolders.ART_ROOT, assetPath);
            if (string.IsNullOrEmpty(replacedFolder))
            {
                return string.Empty;
            }

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(assetPath);
            assetPath = assetPath.Replace($"/{replacedFolder}/", $"/{ConstsFolders.ART_EXPORT_ROOT}/");
            assetPath = assetPath.Replace($"/{fileNameWithoutExtension}.", $"/{newName}.");
            assetPath = PathHelper.Simplify(assetPath);

            return assetPath;
        }
    }
}
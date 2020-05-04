namespace Mayfair.Core.Editor.CustomImportSettings
{
    using System;
    using UnityEngine;

    [Serializable]
    public class FbxImportRules
    {
        #region Settings
        [SerializeField]
        private CustomImportType importType = CustomImportType.None;

        [SerializeField]
        private string rootFolder = null;

        [Header("Containing folder behaviour")]
        [SerializeField]
        private string[] assetFolderIncluded = null;

        [SerializeField]
        private string[] folderIgnored = null;

        [Header("Custom import settings")]
        [SerializeField]
        private ImportSettingsGroup importSettings;
        #endregion

        #region Properties
        public CustomImportType ImportType
        {
            get { return importType; }
        }

        public ImportSettingsGroup ImportSettings
        {
            get { return importSettings; }
        }
        #endregion

        public FbxImportRules(CustomImportType importType, string root, string[] inclusions, string[] exclusions)
        {
            this.importType = importType;
            this.rootFolder = root;
            this.assetFolderIncluded = inclusions;
            this.folderIgnored = exclusions;
        }

        #region Class Methods
        public bool AppliesTo(CustomImportType importType, string importPath)
        {
            return this.importType == importType && importPath.Contains($"/{rootFolder}/");
        }

        public bool ShouldIgnore(string assetPath)
        {
            if (!assetPath.Contains($"/{rootFolder}/"))
            {
                return true;
            }

            foreach (string folder in folderIgnored)
            {
                if (assetPath.Contains($"/{folder}/"))
                {
                    return true;
                }
            }

            bool isValid = false;
            foreach (string folder in assetFolderIncluded)
            {
                if (assetPath.Contains($"/{folder}/"))
                {
                    isValid = true;
                    break;
                }
            }

            return !isValid;
        }
        #endregion
    }
}

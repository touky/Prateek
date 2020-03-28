namespace Mayfair.Core.Editor.AssetLibrary.PostProcessors
{
    using Mayfair.Core.Editor.CustomImportSettings;
    using UnityEngine;

    public class AssetLibraryPostprocessor : CustomAssetPostprocessor
    {
        #region Class Methods
        public override uint GetVersion()
        {
            return 1 + base.GetVersion();
        }

        protected bool ShouldIgnore(CustomImportType importType, GameObject visualSource, string path, bool testLods = true)
        {
            if (testLods && ShouldIgnore(visualSource))
            {
                return true;
            }

            FbxImportRules importRules = AssetLibrary.Get(importType, path);
            if (importRules == null)
            {
                Debug.LogWarning("CustomImportRules couldn't be found, ask your local programmer for help");
                return true;
            }

            if (importRules.ShouldIgnore(assetPath))
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}

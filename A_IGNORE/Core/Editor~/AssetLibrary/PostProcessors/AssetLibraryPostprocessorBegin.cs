namespace Mayfair.Core.Editor.AssetLibrary.PostProcessors
{
    using JetBrains.Annotations;
    using UnityEngine;

    internal class AssetLibraryPostprocessorBegin : CustomAssetPostprocessor
    {
        #region Constructors
        [UsedImplicitly]
        public AssetLibraryPostprocessorBegin() { }
        #endregion

        #region Class Methods
        public override uint GetVersion()
        {
            return 1 + base.GetVersion();
        }

        public override int GetPostprocessOrder()
        {
            return AssetLibraryConsts.PROCESSING_BEGIN;
        }

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            Debug.Log("AssetLibrary Post-processing beginning");
        }
        #endregion
    }
}

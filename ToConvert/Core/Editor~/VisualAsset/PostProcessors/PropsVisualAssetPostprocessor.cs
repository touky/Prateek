namespace Mayfair.Core.Editor.VisualAsset.PostProcessors
{
    using JetBrains.Annotations;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.AssetLibrary.PostProcessors;
    using Mayfair.Core.Editor.CustomImportSettings;
    using UnityEngine;

    public class PropsVisualAssetPostprocessor : AssetLibraryPostprocessor
    {
        #region Constructors
        [UsedImplicitly]
        public PropsVisualAssetPostprocessor() { }
        #endregion

        #region Class Methods
        public override uint GetVersion()
        {
            return 1 + base.GetVersion();
        }

        public override int GetPostprocessOrder()
        {
            return AssetLibraryConsts.PROCESSING_BEGIN + 1;
        }

        private void OnPostprocessModel(GameObject visualSource)
        {
            if (ShouldIgnore(CustomImportType.PropsArt, visualSource, assetPath)) { }

            //todo: benjaminh add custom validator system for props
        }
        #endregion
    }
}

namespace Mayfair.Core.Editor.VisualAsset.PostProcessors
{
    using JetBrains.Annotations;
    using Mayfair.Core.Code.VisualAsset;
    using Mayfair.Core.Editor.Animation;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.AssetLibrary.PostProcessors;
    using Mayfair.Core.Editor.CustomImportSettings;
    using Mayfair.Core.Editor.Utils;
    using UnityEngine;

    public class VisualAssetPostprocessor : AssetLibraryPostprocessor
    {
        #region Static and Constants
        private const string TAKE = "Take ";
        #endregion

        #region Constructors
        [UsedImplicitly]
        public VisualAssetPostprocessor() { }
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
            if (ShouldIgnore(CustomImportType.Buildings, visualSource, assetPath))
            {
                return;
            }

            if (visualSource.name.Contains(ConstsEditor.ANIM_SEPARATOR))
            {
                return;
            }

            Debug.Log($"Adding {typeof(VisualAssetValidator).Name} called for: {visualSource.name} at {assetPath}");

            visualSource.AddComponent<VisualAssetValidator>();
        }

        private void OnPostprocessAnimation(GameObject visualSource, AnimationClip clip)
        {
            if (ShouldIgnore(CustomImportType.Buildings, visualSource, assetPath))
            {
                return;
            }

            Debug.Log($"Adding {typeof(VisualAssetValidator).Name} called for: {visualSource.name} at {assetPath}");

            clip.name = AnimationBuilderHelper.GetAnimationName(visualSource.name);
        }
        #endregion
    }
}

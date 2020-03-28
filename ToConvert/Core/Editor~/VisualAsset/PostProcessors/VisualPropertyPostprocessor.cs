namespace Mayfair.Core.Editor.VisualAsset.PostProcessors
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using JetBrains.Annotations;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.AssetLibrary.PostProcessors;
    using Mayfair.Core.Editor.CustomImportSettings;
    using UnityEngine;

    public class VisualPropertyPostprocessor : AssetLibraryPostprocessor
    {
        #region Static and Constants
        private const string PROPERTY_TILES = "PropertyTiles";
        #endregion

        #region Constructors
        [UsedImplicitly]
        public VisualPropertyPostprocessor() { }
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
            if (ShouldIgnore(CustomImportType.Properties, visualSource, assetPath, false))
            {
                return;
            }

            List<string> matches = new List<string>();
            RegexHelper.TryFetchingMatches(visualSource.name, RegexHelper.AssetTag, matches);
            if (matches.Count == 0)
            {
                return;
            }

            if (!matches.Contains(PROPERTY_TILES))
            {
                return;
            }

            Transform visualTransform = visualSource.transform;
            for (int c = visualTransform.childCount - 1; c >= 0; c--)
            {
                Transform child = visualTransform.GetChild(c);

                //TRICK
                Match match = RegexHelper.AssetTag.Match(child.name);
                if (match.Success && match.Value == "Object")
                {
                    child.name = "PropertyTile_Default_Full";
                }

                AssetLibrary.TryReplacing(child.gameObject);
            }
        }
        #endregion
    }
}

namespace Mayfair.Core.Editor.VisualAsset.PostProcessors
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using JetBrains.Annotations;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Editor.AssetLibrary;
    using Mayfair.Core.Editor.AssetLibrary.PostProcessors;
    using Mayfair.Core.Editor.CustomImportSettings;
    using Mayfair.Core.Editor.ObjectCategorizing;
    using UnityEngine;

    public class VisualAssetBuilderPostprocessor : AssetLibraryPostprocessor
    {
        #region Constructors
        [UsedImplicitly]
        public VisualAssetBuilderPostprocessor() { }
        #endregion

        #region Class Methods
        public override uint GetVersion()
        {
            return 1 + base.GetVersion();
        }

        public override int GetPostprocessOrder()
        {
            return AssetLibraryConsts.PROCESSING_BEGIN + 3;
        }

        private void OnPostprocessModel(GameObject visualSource)
        {
            if (ShouldIgnore(CustomImportType.Buildings, visualSource, assetPath))
            {
                return;
            }

            Debug.Log($"Cleaning up import name for: {visualSource.name} at {assetPath}");

            List<CategorizedInstance> instances = new List<CategorizedInstance>();
            GameObjectCategorizer.Gather(visualSource.gameObject, instances);
            GameObjectCategorizer.Identify(instances);

            foreach (CategorizedInstance instance in instances)
            {
                Match match = RegexHelper.UnityImportRegex.Match(instance.Name);
                if (match.Success)
                {
                    string originalName = instance.Name;
                    instance.Name = instance.Name.Substring(0, match.Index);
                    Debug.Log($" - {originalName} is now {instance.Name}");
                }
            }
        }
        #endregion
    }
}

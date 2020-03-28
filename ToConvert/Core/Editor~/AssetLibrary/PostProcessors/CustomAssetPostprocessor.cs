namespace Mayfair.Core.Editor.AssetLibrary.PostProcessors
{
    using JetBrains.Annotations;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using UnityEditor;
    using UnityEngine;

    public class CustomAssetPostprocessor : AssetPostprocessor
    {
        #region Constructors
        [UsedImplicitly]
        public CustomAssetPostprocessor() { }
        #endregion

        #region Class Methods
        public override uint GetVersion()
        {
            return 1;
        }

        protected bool ShouldIgnore(GameObject visualSource)
        {
            if (visualSource == null)
            {
                return false;
            }

            //If the object imported is already a generated LOD, ignore this entire post process
            if (MeshRegex.GetLODIndex(visualSource.name) != Consts.INDEX_NONE)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}

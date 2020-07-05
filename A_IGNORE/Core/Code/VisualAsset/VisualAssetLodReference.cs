namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class VisualAssetLodReference : MonoBehaviour
    {
        #region Settings
        [SerializeField]
        private List<Transform> animationTransforms = new List<Transform>();

        [SerializeField]
        private MeshRenderer[] lodRenderers;

        [SerializeField]
        [FormerlySerializedAs("filters")]
        private MeshFilter[] lodFilters;
        #endregion

        #region Properties
        public MeshRenderer[] LodRenderers
        {
            get { return lodRenderers; }
        }

        public List<Transform> AnimationTransforms
        {
            get { return animationTransforms; }
        }
        #endregion

        #region Class Methods
        public void SetFiltersContent(VisualAssetLodReference otherLodReference)
        {
            gameObject.SetActive(otherLodReference != null);
            for (int l = 0; l < lodFilters.Length; l++)
            {
                if (otherLodReference != null && l < otherLodReference.lodFilters.Length)
                {
                    lodFilters[l].sharedMesh = otherLodReference.lodFilters[l].sharedMesh;
                }
                else
                {
                    lodFilters[l].sharedMesh = null;
                }
            }
        }

        public void Apply(MaterialPropertyBlock block)
        {
            foreach (MeshRenderer lodRenderer in lodRenderers)
            {
                lodRenderer.SetPropertyBlock(block);
            }
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Animation.AnimatorParameters;
    using Mayfair.Core.Code.Grid;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Code.Utils.Types;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.Playables;
    using UnityEngine.Serialization;

    public class VisualAssetData : MonoBehaviour
    {
        #region Settings
        [SerializeField]
        protected GameObject basePlate;

        [SerializeField]
        private List<Transform> animationTransforms = new List<Transform>();

        [SerializeField]
        [FormerlySerializedAs("lodRefererences")]
        private List<VisualAssetLodReference> lodReferences = new List<VisualAssetLodReference>();
        #endregion

        #region Fields
        private Animator animator;
        private VisualPlayableGraph playableGraph;
        private List<Collider> colliders = new List<Collider>(5);
        private Bounds bounds = new Bounds();
        #endregion

        #region Properties
        public bool IsReady
        {
            get { return playableGraph != null && playableGraph.Controller.IsValid(); }
        }

        public Bounds Bounds
        {
            get { return bounds; }
        }

        public List<Transform> AnimationTransforms
        {
            get { return animationTransforms; }
        }

        public List<VisualAssetLodReference> LodReferences
        {
            get { return lodReferences; }
        }
        #endregion

        #region Class Methods
        public void Init(RuntimeAnimatorController animatorController, AnimatorParameters animationParameters = null)
        {
            using (new ProfilerScope("Awake()"))
            {
                using (new ProfilerScope("GetOrAddComponent()"))
                {
                    animator = gameObject.GetOrAddComponent<Animator>();
                    playableGraph = gameObject.GetOrAddComponent<VisualPlayableGraph>();
                }

                playableGraph.InitGraph(animatorController, lodReferences.Count > 0 ? lodReferences[0].AnimationTransforms : null);

                Assert.IsNotNull(animator, $"Prefab '{name}' does not have an {typeof(Animator).Name}, please check if the prefab has been properly setup.");

                if (animationParameters != null)
                {
                    animationParameters.Validate(playableGraph.Controller);
                }
            }
        }

        public void CacheColliders()
        {
            colliders.Clear();
            GetComponentsInChildren(colliders);

            bounds = new Bounds(SimpleGrid.CELL_CENTER, SimpleGrid.CELL_SIZE_2D);
            if (colliders.Count > 0)
            {
                bounds = ToLocal(colliders[0].bounds);
                for (int c = 1; c < colliders.Count; c++)
                {
                    Bounds localBounds = ToLocal(colliders[c].bounds);
                    bounds.Encapsulate(localBounds.min);
                    bounds.Encapsulate(localBounds.max);
                }
            }
        }

        private Bounds ToLocal(Bounds bounds)
        {
            return new Bounds(transform.InverseTransformPoint(bounds.center), transform.InverseTransformPoint(bounds.extents) * 2);
        }

        public void SetCollidersLayer(int layer)
        {
            gameObject.layer = layer;

            RefreshCollidersLayer();
        }

        protected void RefreshCollidersLayer()
        {
            foreach (Collider collider in colliders)
            {
                collider.gameObject.layer = gameObject.layer;
            }
        }

        public void ShowBasePlate()
        {
            if (basePlate != null)
            {
                basePlate.SetActive(true);
            }
        }

        public void HideBasePlate()
        {
            if (basePlate != null)
            {
                basePlate.SetActive(false);
            }
        }

        public void Apply(MaterialPropertyBlock block)
        {
            foreach (VisualAssetLodReference lodReference in lodReferences)
            {
                lodReference.Apply(block);
            }
        }

        public void Apply(AnimatorPropertyTrigger property)
        {
            property.ApplyTo(playableGraph.Controller);
        }

        public void Apply<TValue>(AnimatorProperty<TValue> property)
        {
            property.ApplyTo(playableGraph.Controller);
        }

        public int AddFullBodyClip(AnimationClip clip)
        {
            return playableGraph.AddFullBodyClip(clip);
        }

        public int AddAdditiveClip(List<AnimationClip> clips)
        {
            return playableGraph.AddAdditiveClip(clips);
        }

        public int AddAdditiveClip(AnimationClip clip)
        {
            return playableGraph.AddAdditiveClip(clip);
        }

        public void PlayBlendTree(int index, float blendTime = 0.1f)
        {
            playableGraph.PlayFullBody(index, blendTime);
        }
        #endregion
    }
}

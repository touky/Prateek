namespace Mayfair.Core.Code.Animation
{
    using System.Collections.Generic;
    using UnityEngine;

    public class AnimationLibrary : ScriptableObject
    {
        #region Settings
        [SerializeField]
        private List<AnimationClip> standardClips = new List<AnimationClip>();

        [SerializeField]
        private List<AnimationClip> additiveClips = new List<AnimationClip>();
        #endregion

        #region Properties
        public IReadOnlyList<AnimationClip> StandardClips
        {
            get { return standardClips; }
        }

        public IReadOnlyList<AnimationClip> AdditiveClips
        {
            get { return additiveClips; }
        }
        #endregion
    }
}

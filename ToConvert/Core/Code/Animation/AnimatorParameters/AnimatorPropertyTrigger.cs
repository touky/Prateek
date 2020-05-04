namespace Mayfair.Core.Code.Animation.AnimatorParameters
{
    using System;
    using UnityEngine;
    using UnityEngine.Animations;

    [Serializable]
    public class AnimatorPropertyTrigger : AnimatorProperty
    {
        #region Constructors
        public AnimatorPropertyTrigger(string property) : base(property) { }
        #endregion

        #region Class Methods
        public static implicit operator AnimatorPropertyTrigger(string property)
        {
            return new AnimatorPropertyTrigger(property);
        }

        public void ApplyTo(Animator animator)
        {
            animator.SetTrigger(ID);
        }

        public void ApplyTo(AnimatorControllerPlayable animator)
        {
            animator.SetTrigger(ID);
        }
        #endregion
    }
}

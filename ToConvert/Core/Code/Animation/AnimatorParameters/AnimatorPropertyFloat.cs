namespace Mayfair.Core.Code.Animation.AnimatorParameters
{
    using System;
    using UnityEngine;
    using UnityEngine.Animations;

    [Serializable]
    public class AnimatorPropertyFloat : AnimatorProperty<float>
    {
        #region Constructors
        public AnimatorPropertyFloat(string property) : base(property) { }
        #endregion

        #region Class Methods
        public static implicit operator AnimatorPropertyFloat(string property)
        {
            return new AnimatorPropertyFloat(property);
        }

        public override void ApplyTo(Animator animator)
        {
            animator.SetFloat(ID, value);
        }

        public override void ApplyTo(AnimatorControllerPlayable animator)
        {
            animator.SetFloat(ID, value);
        }
        #endregion
    }
}

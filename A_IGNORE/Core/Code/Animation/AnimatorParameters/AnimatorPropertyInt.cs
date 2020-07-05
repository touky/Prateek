namespace Mayfair.Core.Code.Animation.AnimatorParameters
{
    using System;
    using UnityEngine;
    using UnityEngine.Animations;

    [Serializable]
    public class AnimatorPropertyInt : AnimatorProperty<int>
    {
        #region Constructors
        public AnimatorPropertyInt(string property) : base(property) { }
        #endregion

        #region Class Methods
        public static implicit operator AnimatorPropertyInt(string property)
        {
            return new AnimatorPropertyInt(property);
        }

        public override void ApplyTo(Animator animator)
        {
            animator.SetInteger(ID, value);
        }

        public override void ApplyTo(AnimatorControllerPlayable animator)
        {
            animator.SetInteger(ID, value);
        }
        #endregion
    }
}

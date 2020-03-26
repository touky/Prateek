namespace Assets.Prateek.ToConvert.NameToId
{
    using System;
    using UnityEngine;
    using UnityEngine.Animations;

    [Serializable]
    public class AnimatorPropertyBool : AnimatorProperty<bool>
    {
        #region Constructors
        public AnimatorPropertyBool(string property) : base(property) { }
        #endregion

        #region Class Methods
        public static implicit operator AnimatorPropertyBool(string property)
        {
            return new AnimatorPropertyBool(property);
        }

        public override void ApplyTo(Animator animator)
        {
            animator.SetBool(ID, value);
        }

        public override void ApplyTo(AnimatorControllerPlayable animator)
        {
            animator.SetBool(ID, value);
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.Animation.AnimatorParameters
{
    using System;
    using UnityEngine;
    using UnityEngine.Animations;

    [Serializable]
    public abstract class AnimatorProperty<TValue> : AnimatorProperty
    {
        #region Fields
        protected TValue value = default;
        #endregion

        #region Properties
        public TValue Value
        {
            get { return value; }
            set { this.value = value; }
        }
        #endregion

        #region Constructors
        public AnimatorProperty(string property) : base(property) { }
        #endregion

        #region Class Methods
        public abstract void ApplyTo(Animator animator);
        public abstract void ApplyTo(AnimatorControllerPlayable animator);
        #endregion
    }
}

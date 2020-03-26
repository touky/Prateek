namespace Assets.Prateek.ToConvert.NameToId
{
    using System;
    using UnityEngine;

    [Serializable]
    public class AnimatorProperty : SystemProperty
    {
        #region Constructors
        public AnimatorProperty(string property) : base(property) { }
        #endregion

        #region Class Methods
        public static implicit operator int(AnimatorProperty property)
        {
            return property.ID;
        }

        public static implicit operator AnimatorProperty(string property)
        {
            return new AnimatorProperty(property);
        }

        protected override void Init()
        {
            if (nameID <= 0)
            {
                nameID = Animator.StringToHash(Name);
            }
        }
        #endregion
    }
}

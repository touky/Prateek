namespace Mayfair.Core.Code.Rendering
{
    using System;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    [Serializable]
    public class ShaderProperty : SystemProperty
    {
        #region Constructors
        public ShaderProperty(string property) : base(property) { }
        #endregion

        #region Class Methods
        public static implicit operator int(ShaderProperty property)
        {
            return property.ID;
        }

        public static implicit operator ShaderProperty(string property)
        {
            return new ShaderProperty(property);
        }

        protected override void Init()
        {
            if (nameID <= 0)
            {
                nameID = Shader.PropertyToID(Name);
            }
        }
        #endregion
    }
}

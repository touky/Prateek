namespace Mayfair.Core.Editor.MeshEditing
{
    using UnityEngine;

    public struct MeshTransformOperation
    {
        #region Fields
        public Matrix4x4 pointMx;
        public UVTransformOperation uvMx;
        #endregion

        #region Constructors
        public MeshTransformOperation(bool value)
        {
            this.pointMx = Matrix4x4.identity;
            this.uvMx = new UVTransformOperation(true);
        }
        #endregion
    }
}

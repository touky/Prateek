namespace Mayfair.Core.Editor.MeshEditing
{
    using UnityEngine;

    public struct UVTransformOperation
    {
        #region Fields
        /// <summary>
        /// Matrices are used by set of two, since uvs are generally Vector2, Vector4 are used as 2 * Vector2
        ///  thus two set of operations can be applied to uvs.
        /// This value is represented by the varialble "uvSet" in the code.
        ///
        /// Note: This systems only supports up to 5 uv sets right now
        /// </summary>
        public Matrix4x4[] matrices;
        #endregion

        #region Properties
        public Matrix4x4 this[int channel]
        {
            get { return this[channel, 0]; }
            set { this[channel, 0] = value; }
        }

        public Matrix4x4 this[int channel, int uvSet]
        {
            get
            {
                Debug.Assert(channel >= 0 && channel < this.matrices.Length);
                Debug.Assert(uvSet >= 0 && uvSet < 2);

                return this.matrices[channel * 2 + uvSet];
            }
            set
            {
                Debug.Assert(channel >= 0 && channel < this.matrices.Length);
                Debug.Assert(uvSet >= 0 && uvSet < 2);

                this.matrices[channel * 2 + uvSet] = value;
            }
        }
        #endregion

        #region Constructors
        public UVTransformOperation(bool value)
        {
            this.matrices = new Matrix4x4[]
            {
                Matrix4x4.identity, Matrix4x4.identity, Matrix4x4.identity, Matrix4x4.identity, Matrix4x4.identity, 
                Matrix4x4.identity, Matrix4x4.identity, Matrix4x4.identity, Matrix4x4.identity, Matrix4x4.identity
            };
        }
        #endregion
    }
}

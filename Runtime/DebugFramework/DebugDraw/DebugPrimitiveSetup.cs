namespace Prateek.Runtime.DebugFramework.DebugDraw
{
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine;

    ///---------------------------------------------------------------------
    public struct DebugPrimitiveSetup
    {
        #region Fields
        ///-----------------------------------------------------------------
        public DebugPrimitiveType type;

        public DebugStyle setup;
        public DebugSpace endDebugSpace;
        public Vector3 pos;
        public Quaternion rot;
        public Vector4 extents;
        public Vector2 range;
        #endregion

        #region Constructors
        ///-----------------------------------------------------------------
        public DebugPrimitiveSetup(DebugPrimitiveType type, DebugStyle setup)
        {
            this.type = type;
            this.setup = setup;
            endDebugSpace = DebugSpace.World;
            pos = Vector3.zero;
            rot = Quaternion.identity;
            extents = Vector3.one;
            range = Statics.vec2(0, 1);
        }
        #endregion
    }
}

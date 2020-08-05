namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-----------------------------------------------------------------------------
    public static partial class Statics
    {
        #region Class Methods
        ///---------------------------------------------------------------------
        public static float length(Vector3Int v)
        {
            return v.magnitude;
        }

        ///---------------------------------------------------------------------
        public static Vector3 Float(Vector3Int v)
        {
            return vec3(v.x, v.y, v.z);
        }
        #endregion
    }
}

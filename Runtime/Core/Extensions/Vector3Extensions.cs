namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static partial class Vector3Extensions
    {
        #region Class Methods
        ///---------------------------------------------------------------------
        public static bool Approximately(this Vector3 v0, Vector3 v1, float epsilon = Vector3.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x
                && d.x < epsilon
                && -epsilon < d.y
                && d.y < epsilon
                && -epsilon < d.z
                && d.z < epsilon;
        }

        ///---------------------------------------------------------------------
        public static float Area(this Vector3 v)
        {
            return v.x * v.y * v.z;
        }

        ///---------------------------------------------------------------------
        public static int ToIndex(this Vector3 v, Vector3 dimensions)
        {
            return (int) v.x + (int) v.y * (int) dimensions.x + (int) v.z * (int) dimensions.xy().Area();
        }

        ///---------------------------------------------------------------------
        public static Vector3 FromIndex(this int index3D, Vector3 dimensions)
        {
            var area2D  = dimensions.xy().Area();
            var index2D = index3D % area2D;
            return new Vector3(index2D % (int) dimensions.x, index2D / (int) dimensions.x, index3D / (int) area2D);
        }
        #endregion
    }
}

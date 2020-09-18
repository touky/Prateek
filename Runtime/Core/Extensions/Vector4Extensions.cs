namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-----------------------------------------------------------------------------
    public static partial class Vector4Extensions
    {
        #region Class Methods
        ///---------------------------------------------------------------------
        public static bool Approximately(this Vector4 v0, Vector4 v1, float epsilon = Vector4.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x
                && d.x < epsilon
                && -epsilon < d.y
                && d.y < epsilon
                && -epsilon < d.z
                && d.z < epsilon
                && -epsilon < d.w
                && d.w < epsilon;
        }

        ///---------------------------------------------------------------------
        public static float Area(this Vector4 v)
        {
            return v.x * v.y * v.z * v.w;
        }

        ///---------------------------------------------------------------------
        public static int ToIndex(this Vector4 v, Vector4 dimensions)
        {
            return (int) v.x + (int) v.y * (int) dimensions.x + (int) v.z * (int) dimensions.xy().Area() + (int) v.w * (int) dimensions.xyz().Area();
        }

        ///---------------------------------------------------------------------
        public static Vector4 FromIndex(this int index4D, Vector4 dimensions)
        {
            var area3D  = dimensions.xyz().Area();
            var area2D  = dimensions.xy().Area();
            var index3D = index4D % area3D;
            var index2D = index3D % area2D;
            return new Vector4(index2D % (int) dimensions.x, index2D / (int) dimensions.x, index3D / (int) area2D, index4D / (int) area3D);
        }
        #endregion
    }
}

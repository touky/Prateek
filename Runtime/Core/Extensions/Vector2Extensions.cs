namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static partial class Vector2Extensions
    {
        #region Class Methods
        ///---------------------------------------------------------------------
        public static bool Approximately(this Vector2 v0, Vector2 v1, float epsilon = Vector2.kEpsilon)
        {
            var d = v0 - v1;
            return -epsilon < d.x
                && d.x < epsilon
                && -epsilon < d.y
                && d.y < epsilon;
        }

        ///---------------------------------------------------------------------
        public static float Area(this Vector2 v)
        {
            return v.x * v.y;
        }

        ///---------------------------------------------------------------------
        public static int ToIndex(this Vector2 v, Vector2 dimensions)
        {
            return (int) v.x + (int) v.y * (int) dimensions.x;
        }

        ///---------------------------------------------------------------------
        public static Vector2 FromIndex(this int index2D, Vector2 dimensions)
        {
            return new Vector2(index2D % (int) dimensions.x, index2D / (int) dimensions.x);
        }
        #endregion
    }
}

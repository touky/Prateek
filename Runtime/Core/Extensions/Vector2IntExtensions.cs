namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;
    using static Statics;

    ///-----------------------------------------------------------------------------
    public static partial class Vector2IntExtensions
    {
        ///---------------------------------------------------------------------
        public static int Area(this Vector2Int v)
        {
            return v.x * v.y;
        }

        ///---------------------------------------------------------------------
        public static int ToIndex(this Vector2Int v, Vector2Int dimensions)
        {
            return (int) v.x + (int) v.y * (int) dimensions.x;
        }

        ///---------------------------------------------------------------------
        public static Vector2Int FromIndex(this int index2D, Vector2Int dimensions)
        {
            return new Vector2Int(index2D % dimensions.x, index2D / dimensions.x);
        }
    }
}

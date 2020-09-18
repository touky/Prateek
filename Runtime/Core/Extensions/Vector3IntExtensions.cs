namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-----------------------------------------------------------------------------
    public static partial class Vector3IntExtensions
    {
        #region Class Methods
        ///---------------------------------------------------------------------
        public static int Area(this Vector3Int v)
        {
            return v.x * v.y * v.z;
        }

        ///---------------------------------------------------------------------
        public static int ToIndex(this Vector3Int v, Vector3Int dimensions)
        {
            return v.x + v.y * dimensions.x + v.z * dimensions.xy().Area();
        }

        ///---------------------------------------------------------------------
        public static Vector3Int FromIndex(this int index3D, Vector3Int dimensions)
        {
            var area2D  = dimensions.xy().Area();
            var index2D = index3D % area2D;
            return new Vector3Int(index2D % dimensions.x, index2D / dimensions.x, index3D / area2D);
        }
        #endregion
    }
}

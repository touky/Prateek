namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    public static partial class Statics
    {
        #region Class Methods
        public static float length(Vector2Int v)
        {
            return v.magnitude;
        }

        public static Vector2 Float(Vector2Int v)
        {
            return vec2(v.x, v.y);
        }
        #endregion
    }
}

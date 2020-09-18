namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static partial class Statics
    {
        #region Class Methods
        public static Vector4 normalize(Vector4 v)
        {
            return v.normalized;
        }

        public static float length(Vector4 v)
        {
            return v.magnitude;
        }

        public static float dot(Vector4 v0, Vector4 v1)
        {
            return Vector4.Dot(v0, v1);
        }

        public static Vector4 lerp(Vector4 v0, Vector4 v1, float alpha)
        {
            return Vector4.Lerp(v0, v1, alpha);
        }

        public static Vector4 mix(Vector4 v0, Vector4 v1, float alpha)
        {
            return Vector4.Lerp(v0, v1, alpha);
        }

        public static Vector3Int Int(Vector4 v, out int w)
        {
            w = (int) v.w;
            return vec3i((int) v.x, (int) v.y, (int) v.z);
        }
        #endregion
    }
}

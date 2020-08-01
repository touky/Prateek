namespace Prateek.Core.Code.Extensions
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static partial class CSharp
    {
        public static Vector3 normalize(Vector3 v) { return v.normalized; }
        public static float length(Vector3 v) { return v.magnitude; }
        public static float dot(Vector3 v0, Vector3 v1) { return Vector3.Dot(v0, v1); }
        public static Vector3 lerp(Vector3 v0, Vector3 v1, float alpha) { return Vector3.Lerp(v0, v1, alpha); }
        public static Vector3 mix(Vector3 v0, Vector3 v1, float alpha) { return Vector3.Lerp(v0, v1, alpha); }
        public static Vector3Int Int(Vector3 v) { return Extensions.CSharp.vec3i((int)v.x, (int)v.y, (int)v.z); }
        public static Vector3 cross(Vector3 v0, Vector3 v1) { return Vector3.Cross(v0, v1); }
    }
}

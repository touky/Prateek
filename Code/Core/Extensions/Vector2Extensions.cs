namespace Prateek.Core.Code.Extensions
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public static partial class CSharp
    {
        public static Vector2 normalize(Vector2 v) { return v.normalized; }
        public static float length(Vector2 v) { return v.magnitude; }
        public static float dot(Vector2 v0, Vector2 v1) { return Vector2.Dot(v0, v1); }
        public static Vector2 lerp(Vector2 v0, Vector2 v1, float alpha) { return Vector2.Lerp(v0, v1, alpha); }
        public static Vector2 mix(Vector2 v0, Vector2 v1, float alpha) { return Vector2.Lerp(v0, v1, alpha); }
        public static Vector2Int Int(Vector2 v) { return Extensions.CSharp.vec2i((int)v.x, (int)v.y); }
    }
}

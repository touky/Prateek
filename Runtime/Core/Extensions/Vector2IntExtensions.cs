namespace Prateek.Runtime.Core.Extensions
{
    using UnityEngine;
    using static Statics;

    public static partial class Statics
    {
        public static float length(Vector2Int v) { return v.magnitude; }
        public static Vector2 Float(Vector2Int v) { return vec2(v.x, v.y); }
    }
}

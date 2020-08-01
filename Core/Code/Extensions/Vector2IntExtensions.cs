namespace Prateek.Core.Code.Extensions
{
    using UnityEngine;

    public static partial class CSharp
    {
        public static float length(Vector2Int v) { return v.magnitude; }
        public static Vector2 Float(Vector2Int v) { return Extensions.CSharp.vec2(v.x, v.y); }
    }
}

namespace Prateek.Core.Code.Extensions
{
    using UnityEngine;

    public static partial class CSharp
    {
        public static float length(Vector3Int v) { return v.magnitude; }
        public static Vector3 Float(Vector3Int v) { return Extensions.CSharp.vec3(v.x, v.y, v.z); }
    }
}

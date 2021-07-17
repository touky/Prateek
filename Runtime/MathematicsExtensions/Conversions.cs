

//#pragma warning disable 0660, 0661

namespace Prateek.Runtime.MathematicsExtensions
{
    using Unity.Mathematics;
    using UnityEngine;

    public static class MathematicsExtensions
    {
        public static Vector2Int Unity(int2 v) { return new Vector2Int(v.x, v.y); }
        public static Vector3Int Unity(int3 v) { return new Vector3Int(v.x, v.y, v.z); }
        public static Vector2 Unity(float2 v) { return new Vector2(v.x, v.y); }
        public static Vector3 Unity(float3 v) { return new Vector3(v.x, v.y, v.z); }
        public static Vector4 Unity(float4 v) { return new Vector4(v.x, v.y, v.z, v.w); }
        public static Quaternion Unity(quaternion q) { return new Quaternion(q.value.x, q.value.y, q.value.z, q.value.w); }
        public static Matrix4x4 Unity(float4x4 m) { return new Matrix4x4((Vector4)m.c0, (Vector4)m.c1, (Vector4)m.c2, (Vector4)m.c3); }

        public static int2 Math(Vector2Int v) { return new int2(v.x, v.y); }
        public static int3 Math(Vector3Int v) { return new int3(v.x, v.y, v.z); }
        public static float2 Math(Vector2 v) { return new float2(v.x, v.y); }
        public static float3 Math(Vector3 v) { return new float3(v.x, v.y, v.z); }
        public static float4 Math(Vector4 v) { return new float4(v.x, v.y, v.z, v.w); }
        public static quaternion Math(Quaternion q) { return new quaternion(q.x, q.y, q.z, q.w); }
        public static float4x4 Math(Matrix4x4 m) { return new float4x4((float4)m.GetColumn(0), (float4)m.GetColumn(1), (float4)m.GetColumn(2), (float4)m.GetColumn(3)); }
    }
}

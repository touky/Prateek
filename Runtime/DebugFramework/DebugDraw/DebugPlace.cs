namespace Prateek.Runtime.DebugFramework.DebugDraw
{
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine;
    using Statics = Core.Statics.Statics;

    ///---------------------------------------------------------------------
    public struct DebugPlace
    {
        ///-----------------------------------------------------------------
        public enum Pivot
        {
            Center,
            Bottom,
        }

        ///-----------------------------------------------------------------
        private Vector3 position;

        private Quaternion rotation;
        private Vector3 extents;

        ///-----------------------------------------------------------------
        public Vector3 Start { get { return position - rotation * extents.nnz(); } }

        public Vector3 End { get { return position + rotation * extents.nnz(); } }
        public Vector3 Position { get { return position; } }
        public Quaternion Rotation { get { return rotation; } }
        public Vector3 Forward { get { return rotation * Vector3.forward; } }
        public Vector3 Up { get { return rotation * Vector3.up; } }
        public Vector3 Right { get { return rotation * Vector3.right; } }
        public Vector3 Extents { get { return extents; } }
        public Vector3 Size { get { return extents * 2; } }

        ///-----------------------------------------------------------------
        public DebugPlace(bool none)
        {
            position = Vector3.zero;
            rotation = Quaternion.identity;
            extents = Vector3.one;
        }

        ///-----------------------------------------------------------------
        public static DebugPlace AToB(Vector3 a, Vector3 b) { return AToB(Pivot.Center, a, b, Statics.vec3(0), Vector3.up); }

        public static DebugPlace AToB(Vector3 a, Vector3 b, Vector3 up) { return AToB(Pivot.Center, a, b, Statics.vec3(0), Vector3.up); }

        public static DebugPlace AToB(Vector3 a, Vector3 b, float size) { return AToB(Pivot.Center, a, b, Statics.vec3(size), Vector3.up); }

        public static DebugPlace AToB(Vector3 a, Vector3 b, float size, Vector3 up) { return AToB(Pivot.Center, a, b, Statics.vec3(size), up); }

        public static DebugPlace AToB(Vector3 a, Vector3 b, Vector2 size, Vector3 up) { return AToB(Pivot.Center, a, b, size.yyx(), up); }

        public static DebugPlace AToB(Vector3 a, Vector3 b, Vector3 size, Vector3 up) { return AToB(Pivot.Center, a, b, size, up); }

        ///-----------------------------------------------------------------
        public static DebugPlace AToB(Pivot pivot, Vector3 a, Vector3 b) { return AToB(pivot, a, b, Statics.vec3(0), Vector3.up); }

        public static DebugPlace AToB(Pivot pivot, Vector3 a, Vector3 b, Vector3 up) { return AToB(pivot, a, b, Statics.vec3(0), Vector3.up); }

        public static DebugPlace AToB(Pivot pivot, Vector3 a, Vector3 b, float size) { return AToB(pivot, a, b, Statics.vec3(size), Vector3.up); }

        public static DebugPlace AToB(Pivot pivot, Vector3 a, Vector3 b, float size, Vector3 up) { return AToB(pivot, a, b, Statics.vec3(size), up); }

        public static DebugPlace AToB(Pivot pivot, Vector3 a, Vector3 b, Vector2 size, Vector3 up) { return AToB(pivot, a, b, size.yyx(), up); }

        public static DebugPlace AToB(Pivot pivot, Vector3 a, Vector3 b, Vector3 size, Vector3 up)
        {
            var offset = pivot == Pivot.Center ? Vector3.zero : up * size.y * 0.5f;
            var dir    = b - a;
            return new DebugPlace(true)
            {
                position = a + dir * 0.5f + offset,
                rotation = Quaternion.LookRotation(Statics.normalize(dir), up),
                extents = Statics.vec3(0, 0, Statics.length(dir) * 0.5f) + size * 0.5f
            };
        }

        ///-----------------------------------------------------------------
        public static DebugPlace Ray(Vector3 position, float distance) { return Ray(Pivot.Center, position, Vector3.forward, Statics.vec3(distance), Vector3.up); }

        public static DebugPlace Ray(Vector3 position, Vector3 dir, float distance) { return Ray(Pivot.Center, position, dir, Statics.vec3(distance), Vector3.up); }

        public static DebugPlace Ray(Vector3 position, Vector3 dir, float distance, Vector3 up) { return Ray(Pivot.Center, position, dir, Statics.vec3(distance), up); }

        public static DebugPlace Ray(Vector3 position, Vector3 dir, Vector2 size, Vector3 up) { return Ray(Pivot.Center, position, dir, size.yyx(), up); }

        public static DebugPlace Ray(Vector3 position, Vector3 dir, Vector3 size, Vector3 up) { return Ray(Pivot.Center, position, dir, size, up); }

        ///-----------------------------------------------------------------
        public static DebugPlace Ray(Pivot pivot, Vector3 position, float distance) { return Ray(pivot, position, Vector3.forward, Statics.vec3(distance), Vector3.up); }

        public static DebugPlace Ray(Pivot pivot, Vector3 position, Vector3 dir, float distance) { return Ray(pivot, position, dir, Statics.vec3(distance), Vector3.up); }

        public static DebugPlace Ray(Pivot pivot, Vector3 position, Vector3 dir, float distance, Vector3 up) { return Ray(pivot, position, dir, Statics.vec3(distance), up); }

        public static DebugPlace Ray(Pivot pivot, Vector3 position, Vector3 dir, Vector2 size, Vector3 up) { return Ray(pivot, position, dir, size.yyx(), up); }

        public static DebugPlace Ray(Pivot pivot, Vector3 position, Vector3 dir, Vector3 size, Vector3 up)
        {
            var offset = pivot == Pivot.Center ? Vector3.zero : up * size.y * 0.5f;
            return new DebugPlace(true)
            {
                position = position + dir * size.z * 0.5f + offset,
                rotation = Quaternion.LookRotation(dir, up),
                extents = size * 0.5f
            };
        }

        ///-----------------------------------------------------------------
        public static DebugPlace At(float size) { return At(Pivot.Center, Vector3.zero, Quaternion.identity, Statics.vec3(size)); }

        public static DebugPlace At(Vector2 size) { return At(Pivot.Center, Vector3.zero, Quaternion.identity, size.yyx()); }

        public static DebugPlace At(Vector3 size) { return At(Pivot.Center, Vector3.zero, Quaternion.identity, size); }

        public static DebugPlace At(Vector3 position, float size) { return At(Pivot.Center, position, Quaternion.identity, Statics.vec3(size)); }

        public static DebugPlace At(Vector3 position, Vector2 size) { return At(Pivot.Center, position, Quaternion.identity, size.yyx()); }

        public static DebugPlace At(Vector3 position, Vector3 size) { return At(Pivot.Center, position, Quaternion.identity, size); }

        public static DebugPlace At(Vector3 position, Quaternion rotation, float size) { return At(Pivot.Center, position, rotation, Statics.vec3(size)); }

        public static DebugPlace At(Vector3 position, Quaternion rotation, Vector2 size) { return At(Pivot.Center, position, rotation, size.yyx()); }

        public static DebugPlace At(Vector3 position, Quaternion rotation, Vector3 size) { return At(Pivot.Center, position, rotation, size); }

        ///-----------------------------------------------------------------
        public static DebugPlace At(Pivot pivot, float size) { return At(pivot, Vector3.zero, Quaternion.identity, Statics.vec3(size)); }

        public static DebugPlace At(Pivot pivot, Vector2 size) { return At(pivot, Vector3.zero, Quaternion.identity, size.yyx()); }

        public static DebugPlace At(Pivot pivot, Vector3 size) { return At(pivot, Vector3.zero, Quaternion.identity, size); }

        public static DebugPlace At(Pivot pivot, Vector3 position, float size) { return At(pivot, position, Quaternion.identity, Statics.vec3(size)); }

        public static DebugPlace At(Pivot pivot, Vector3 position, Vector2 size) { return At(Pivot.Center, position, Quaternion.identity, size.yyx()); }

        public static DebugPlace At(Pivot pivot, Vector3 position, Vector3 size) { return At(pivot, position, Quaternion.identity, size); }

        public static DebugPlace At(Pivot pivot, Vector3 position, Quaternion rotation, float size) { return At(pivot, position, rotation, Statics.vec3(size)); }

        public static DebugPlace At(Pivot pivot, Vector3 position, Quaternion rotation, Vector2 size) { return At(pivot, position, rotation, size.yyx()); }

        public static DebugPlace At(Pivot pivot, Vector3 position, Quaternion rotation, Vector3 size)
        {
            var offset = pivot == Pivot.Center ? Vector3.zero : rotation * Vector3.up * size.y * 0.5f;
            return new DebugPlace(true)
            {
                position = position + offset,
                rotation = rotation,
                extents = size * 0.5f
            };
        }

        ///-----------------------------------------------------------------
        public static DebugPlace Bounds(Bounds bounds) { return Bounds(Pivot.Center, bounds, Quaternion.identity); }

        public static DebugPlace Bounds(Bounds bounds, Quaternion rotation) { return Bounds(Pivot.Center, bounds, rotation); }

        ///-----------------------------------------------------------------
        public static DebugPlace Bounds(Pivot pivot, Bounds bounds) { return Bounds(pivot, bounds, Quaternion.identity); }

        public static DebugPlace Bounds(Pivot pivot, Bounds bounds, Quaternion rotation)
        {
            var offset = pivot == Pivot.Center ? Vector3.zero : rotation * Vector3.up * bounds.size.y * 0.5f;
            return new DebugPlace(true)
            {
                position = bounds.center + offset,
                rotation = rotation,
                extents = bounds.extents
            };
        }
    }
}

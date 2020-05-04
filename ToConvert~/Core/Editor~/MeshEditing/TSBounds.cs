namespace Mayfair.Core.Editor.MeshEditing
{
    using System;
    using UnityEngine;

    [Serializable]
    public struct TSBounds
    {
        #region Fields
        public Vector3 center;
        public Vector3 extents;
        #endregion

        #region Properties
        public Vector3 Max
        {
            get { return this.center + this.extents; }
        }

        public Vector3 Min
        {
            get { return this.center - this.extents; }
        }

        public Vector3 Size
        {
            get { return this.extents * 2; }
        }
        #endregion

        #region Constructors
        public TSBounds(Bounds bounds)
        {
            this.center = bounds.center;
            this.extents = bounds.extents;
        }

        public TSBounds(Vector3 center, Vector3 size)
        {
            this.center = center;
            this.extents = size / 2;
        }

        public TSBounds(Vector3 min, Vector3 max, Vector3 center)
        {
            Vector3 size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
            this.extents = size / 2f;
            this.center = center;
        }
        #endregion

        #region Class Methods
        public void CreateFromMinMax(Vector3 min, Vector3 max)
        {
            Vector3 size = new Vector3(max.x - min.x, max.y - min.y, max.z - min.z);
            this.extents = size / 2f;
            this.center = Vector3.Lerp(min, max, 0.5f);
        }

        public Vector3 ClosestPoint(Vector3 point)
        {
            return Vector3.Max(Min, Vector3.Min(point, Max));
        }

        public bool Contains(Vector3 point)
        {
            return Mathf.Abs(point.x) <= this.extents.x
                && Mathf.Abs(point.y) <= this.extents.y
                && Mathf.Abs(point.z) <= this.extents.z;
        }
        #endregion
    }
}

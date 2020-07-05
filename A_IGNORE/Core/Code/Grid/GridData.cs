namespace Mayfair.Core.Code.Grid
{
    using System;
    using UnityEngine;

    [Serializable]
    public abstract class GridData : GridFootprint
    {
        #region Fields
        private Vector2 blockSize;
        #endregion Fields

        #region Properties
        #endregion Properties

        #region Class Methods
        public abstract Bounds GetBoundingBox();
        public abstract bool Contains(GridFootprint footprint);
        public abstract bool Contains(Vector2Int localPoint);

        public abstract void LocalToGrid(Vector2 localPosition, out Vector2Int gridPoint);
        public abstract void LocalToGrid(Vector2 localPosition, Quaternion localRotation, out Vector2Int gridPoint, out Quaternion gridRotation);
        public abstract void LocalToGrid(Vector2 localPosition, Quaternion localRotation, out Vector2Int gridPoint, out Vector2Int gridDirection);

        public abstract void GridToLocal(Vector2Int gridPoint, out Vector2 localPosition);
        public abstract void GridToLocal(Vector2Int gridPoint, Quaternion gridRotation, out Vector2 localPosition, out Quaternion localRotation);
        public abstract void GridToLocal(Vector2Int gridPoint, Vector2Int gridDirection, out Vector2 localPosition, out Quaternion localRotation);

        public abstract bool TestMarking(Vector2 localPosition, Quaternion localRotation, GridFootprint footprint);
        public abstract bool TryMarking(Vector2 localPosition, Quaternion localRotation, GridFootprint footprint);
        public abstract bool TestMarking(Vector2Int gridPoint, Vector2Int gridDirection, GridFootprint footprint);
        public abstract bool TryMarking(Vector2Int gridPoint, Vector2Int gridDirection, GridFootprint footprint);
        #endregion Class Methods
    }
}
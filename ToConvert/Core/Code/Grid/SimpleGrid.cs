namespace Mayfair.Core.Code.Grid
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SimpleGrid
    {
        #region Static and Constants
        public static readonly Vector3 CELL_SIZE_2D = new Vector3(1, 0, 1);
        public static readonly Vector3 CELL_SIZE_3D = new Vector3(1, 1, 1);
        public static readonly Vector3 CELL_CENTER = CELL_SIZE_2D * 0.5f;
        #endregion

        #region Settings
        [SerializeField]
        [HideInInspector]
        private Vector3 cellSize = CELL_SIZE_2D;

        [SerializeField]
        private Transform sourceTransform;
        #endregion

        #region Constructors
        public SimpleGrid() { }

        public SimpleGrid(Transform sourceTransform) : this()
        {
            Init(sourceTransform);
        }
        #endregion

        #region Class Methods
        public void Init(Transform sourceTransform)
        {
            this.sourceTransform = sourceTransform;
        }

        public Vector3 WorldToLocal(Vector3 worldPosition)
        {
            return sourceTransform.InverseTransformPoint(worldPosition);
        }

        public Vector3Int LocalToCell(Vector3 localPosition)
        {
            return new Vector3Int(
                cellSize.x == 0 ? 0 : Mathf.RoundToInt(localPosition.x / cellSize.x),
                cellSize.y == 0 ? 0 : Mathf.RoundToInt(localPosition.y / cellSize.y),
                cellSize.z == 0 ? 0 : Mathf.RoundToInt(localPosition.z / cellSize.z)
            );
        }

        public Vector3 LocalToWorld(Vector3 localPosition)
        {
            return sourceTransform.TransformPoint(localPosition);
        }

        public Vector3 CellToLocal(Vector3Int cellPosition)
        {
            return new Vector3(cellPosition.x * cellSize.x, cellPosition.y * cellSize.y, cellPosition.z * cellSize.z);
        }

        public Vector3 CellToLocalCenter(Vector3Int cellPosition)
        {
            return new Vector3(cellPosition.x * cellSize.x, cellPosition.y * cellSize.y, cellPosition.z * cellSize.z) + cellSize * 0.5f;
        }

        public Vector3Int WorldToCell(Vector3 worldPosition)
        {
            return LocalToCell(WorldToLocal(worldPosition));
        }

        public Vector3 CellToWorld(Vector3Int cellPosition)
        {
            return LocalToWorld(CellToLocal(cellPosition));
        }

        public Vector3 CellToWorldCenter(Vector3Int cellPosition)
        {
            return LocalToWorld(CellToLocalCenter(cellPosition));
        }
        #endregion
    }
}

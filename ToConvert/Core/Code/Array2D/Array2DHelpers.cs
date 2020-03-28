namespace Mayfair.Core.Code.Utils.Types
{
    using UnityEngine;

    public static class Array2DHelpers
    {
        #region Class Methods
        public static Vector2Int IndexToCoord(int index, Vector2Int size)
        {
            Debug.Assert(index >= 0 && index < size.x * size.y);

            return new Vector2Int(index % size.x, index / size.x);
        }

        public static int CoordToIndex(Vector2Int position, Vector2Int size)
        {
            return CoordToIndex(position.x, position.y, size);
        }

        public static int CoordToIndex(int x, int y, Vector2Int size)
        {
            Debug.Assert(x >= 0 && y >= 0 && x < size.x && x < size.y);

            return x + y * size.x;
        }
        #endregion
    }
}

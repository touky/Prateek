namespace Mayfair.Core.Code.Utils
{
    using UnityEngine;

    public static class VectorUtils
    {
        public static Vector2Int Rotate90DegreeClockwise(Vector2Int vector)
        {
            return new Vector2Int(vector.y, -vector.x);
        }

        public static Vector2Int RotateCell(Vector2Int origin, int rotationValue)
        {
            // We use vector3.Back cause the rotation has to be clockwise and it's counter clockwise by default
            Quaternion rotation = Quaternion.Euler(Vector3.back * rotationValue);
            Vector3 x = rotation * Vector3.right;
            Vector3 y = rotation * Vector3.up;
            Vector3 rotatedCell = x * origin.x + y * origin.y;

            return new Vector2Int(Mathf.RoundToInt(rotatedCell.x), Mathf.RoundToInt(rotatedCell.y));
        }
    }
}

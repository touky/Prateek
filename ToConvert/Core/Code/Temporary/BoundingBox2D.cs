namespace Mayfair.Core.Code.Temporary
{
    using UnityEngine;

    public struct BoundingBox2D
    {
        public Vector2 min;
        public Vector2 max;

        public BoundingBox2D(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }

        public Vector2 Clamp(Vector2 point)
        {
            return new Vector2(
                Mathf.Clamp(point.x, this.min.x, this.max.x),
                Mathf.Clamp(point.y, this.min.y, this.max.y)
            );
        }

        public Vector3 Clamp(Vector3 wsPoint)
        {
            return new Vector3(
                Mathf.Clamp(wsPoint.x, this.min.x, this.max.x),
                Mathf.Clamp(wsPoint.y, this.min.y, this.max.y),
                wsPoint.z
            );
        }
    }
}
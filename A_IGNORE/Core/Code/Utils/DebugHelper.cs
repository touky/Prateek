namespace Mayfair.Core.Code.Utils
{
    using System.Diagnostics;
    using UnityEngine;

    public static class DebugHelper
    {
        #region Static and Constants
        private static Vector3[] boxPoints = new Vector3[8];

        private static int[] boxIndices =
        {
            0, 1, 2, 3, 4, 5, 6, 7,
            0, 2, 1, 3, 4, 6, 5, 7,
            0, 4, 1, 5, 2, 6, 3, 7
        };
        #endregion

        #region Class Methods
        [Conditional("PRATEEK_DEBUG")]
        public static void DrawDebugRay(Ray ray, RaycastHit hit, Color color, float duration = -1)
        {
            UnityEngine.Debug.DrawLine(ray.origin, hit.point, color, duration);
            UnityEngine.Debug.DrawLine(hit.point, hit.point + ray.direction * 10, Color.grey, duration);
        }

        [Conditional("PRATEEK_DEBUG")]
        public static void DrawDebugRay(Ray ray, Color color, float duration = -1)
        {
            UnityEngine.Debug.DrawRay(ray.origin, ray.direction * 100, color, duration);
        }

        [Conditional("PRATEEK_DEBUG")]
        public static void DrawCrossAtPosition(Vector3 position, Color color, float yRotation = 0, float yOffset = 0, float duration = -1)
        {
            Quaternion rotation = Quaternion.Euler(0, yRotation, 0);
            Vector3 back = rotation * Vector3.back;
            Vector3 forward = rotation * Vector3.forward;
            Vector3 left = rotation * Vector3.left;
            Vector3 right = rotation * Vector3.right;
            UnityEngine.Debug.DrawLine(position + Vector3.up * yOffset + back * 0.5f, position + Vector3.up * yOffset + forward * 0.5f, color, duration);
            UnityEngine.Debug.DrawLine(position + Vector3.up * yOffset + left * 0.5f, position + Vector3.up * yOffset + right * 0.5f, color, duration);
        }

        [Conditional("PRATEEK_DEBUG")]
        public static void DrawBoxGrounded(Vector3 position, Quaternion rotation, Vector3 extents, Color color, float duration = -1)
        {
            Vector3 size = extents * 2;
            Bounds bounds = new Bounds(Vector3.zero, size);

            Vector3 min = bounds.min;
            boxPoints[0] = min + extents.y * Vector3.up;
            boxPoints[1] = boxPoints[0] + size.z * Vector3.forward;
            boxPoints[2] = boxPoints[0] + size.x * Vector3.right;
            boxPoints[3] = boxPoints[1] + size.x * Vector3.right;
            boxPoints[4] = boxPoints[0] + size.y * Vector3.up;
            boxPoints[5] = boxPoints[1] + size.y * Vector3.up;
            boxPoints[6] = boxPoints[2] + size.y * Vector3.up;
            boxPoints[7] = boxPoints[3] + size.y * Vector3.up;

            for (int p = 0; p < boxPoints.Length; p++)
            {
                boxPoints[p] = position + rotation * boxPoints[p];
            }

            for (int i = 0; i < boxIndices.Length; i += 2)
            {
                UnityEngine.Debug.DrawLine(boxPoints[boxIndices[i]], boxPoints[boxIndices[i + 1]], color, duration, true);
            }
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.Utils
{
    using Mayfair.Core.Code.Input;
    using UnityEngine;

    public static class RaycastHelper
    {
        #region Class Methods
        public static bool GetGroundPosition(out Vector3 groundPosition, RaycastInfo raycastInfo, LayerMask mainLayer, LayerMask fallbackLayer)
        {
            RaycastHit hit = raycastInfo[mainLayer];
            if (hit.collider == null)
            {
                hit = raycastInfo[fallbackLayer];
            }

            groundPosition = hit.point;
            if (hit.collider == null)
            {
                return false;
            }

            return GetGroundPosition(out groundPosition, hit.collider.transform.position.y, raycastInfo.Ray, hit);
        }

        public static bool GetGroundPosition(out Vector3 groundPosition, float groundHeight, RaycastInfo raycastInfo, LayerMask mainLayer, LayerMask fallbackLayer)
        {
            RaycastHit hit = raycastInfo[mainLayer];
            if (hit.collider == null)
            {
                hit = raycastInfo[fallbackLayer];
            }

            groundPosition = hit.point;
            return GetGroundPosition(out groundPosition, groundHeight, raycastInfo.Ray, hit);
        }

        public static bool GetGroundPosition(out Vector3 groundPosition, Ray ray, RaycastHit hit)
        {
            groundPosition = hit.point;
            if (hit.collider == null)
            {
                return false;
            }

            return GetGroundPosition(out groundPosition, hit.collider.transform.position.y, ray, hit);
        }

        public static bool GetGroundPosition(out Vector3 groundPosition, float groundHeight, Ray ray, RaycastHit hit)
        {
            groundPosition = hit.point;
            if (hit.collider == null)
            {
                return false;
            }

            //Adjusting the ground position by extending the ray hit up to the actual ground
            //As the Collider can have a custom size, we only collide on the skin of said collider, thus a bit away from our destination
            //And y = 0 is a bit too simple to do the trick
            groundPosition = hit.point + ray.direction * (hit.point.y - groundHeight) / Vector3.Dot(ray.direction, Vector3.down);

            return true;
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.Temporary
{
    using UnityEngine;

    public class CameraUtilities : MonoBehaviour
    {
        #region Class Methods
        public static Camera GetCamera()
        {
            return Camera.main;
        }

        public static Vector3 ScreenToGround(Vector2 screenInputPosition, Camera cam = null, float distance = 0f)
        {
            if (cam == null)
            {
                cam = Camera.main;
            }

            Plane groundPlane = new Plane(new Vector3(0.0f, 1.0f, 0.0f), -distance);

            Ray ray = cam.ScreenPointToRay(screenInputPosition);
            float rayDistance;
            groundPlane.Raycast(ray, out rayDistance);
            Vector3 result = ray.GetPoint(rayDistance);
            result.y = 0;
            return result;
        }

        public static RaycastHit RaycastFromInput(Vector2 screenInputPosition)
        {
            return RaycastFromInput(screenInputPosition, new RaycastSetup(float.MaxValue));
        }

        public static RaycastHit RaycastFromInput(Vector2 screenInputPosition, RaycastSetup setup)
        {
            Ray ray;
            return RaycastFromInput(screenInputPosition, setup, out ray);
        }

        public static RaycastHit RaycastFromInput(Vector2 screenInputPosition, RaycastSetup setup, out Ray ray)
        {
            Camera cam = setup.Camera;
            if (cam == null)
            {
                cam = Camera.main;
            }

            RaycastHit raycastHit = new RaycastHit();
            ray = cam.ScreenPointToRay(screenInputPosition);
            if (Physics.Raycast(ray, out raycastHit, setup.Distance, setup.Layer))
            {
                return raycastHit;
            }

            return new RaycastHit();
        }

        public static Collider[] RaycastAllFromInput(Vector2 screenInputPosition, Camera cam = null)
        {
            if (cam == null)
            {
                cam = Camera.main;
            }

            Ray ray = cam.ScreenPointToRay(screenInputPosition);
            RaycastHit[] raycastHits = Physics.RaycastAll(ray);
            int length = raycastHits.Length;
            Collider[] colliders = new Collider[length];

            for (int i = 0; i < length; ++i)
            {
                colliders[i] = raycastHits[i].collider;
            }

            return colliders;
        }

        public static void RenderAsBillboard(GameObject sceneObject, Vector3 baseScale, Camera cam = null)
        {
            if (cam == null)
            {
                cam = Camera.main;
            }

            //look at camera
            sceneObject.transform.LookAt(sceneObject.transform.position + cam.transform.rotation * Vector3.forward,
                                         cam.transform.rotation * Vector3.up);

            //scale to stay the same size no matter how far
            float cameraDistance = Vector3.Dot(sceneObject.transform.position - cam.transform.position, cam.transform.forward);
            sceneObject.transform.localScale = baseScale * cameraDistance;
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.Temporary
{
    using UnityEngine;

    public struct RaycastSetup
    {
        #region Fields
        private Camera camera;
        private float distance;
        private int layer;
        #endregion

        #region Properties
        public Camera Camera
        {
            get { return camera; }
        }

        public float Distance
        {
            get { return distance; }
        }

        public int Layer
        {
            get { return layer; }
        }
        #endregion

        #region Constructors
        private RaycastSetup(bool defaultSetup)
        {
            camera = null;
            distance = float.MaxValue;
            layer = ~0;
        }

        public RaycastSetup(Camera camera, float distance, int layer) : this(true)
        {
            this.camera = camera;
            this.distance = distance;
            this.layer = layer;

            GrabCamera();
        }

        public RaycastSetup(Camera camera) : this(true)
        {
            this.camera = camera;

            GrabCamera();
        }

        public RaycastSetup(float distance) : this(true)
        {
            this.distance = distance;

            GrabCamera();
        }

        public RaycastSetup(int layer) : this(true)
        {
            this.layer = layer;

            GrabCamera();
        }

        public RaycastSetup(float distance, int layer) : this(true)
        {
            this.distance = distance;
            this.layer = layer;

            GrabCamera();
        }
        #endregion

        #region Class Methods
        private void GrabCamera()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
        }
        #endregion
    }
}

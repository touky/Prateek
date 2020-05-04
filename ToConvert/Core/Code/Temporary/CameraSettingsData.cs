namespace Mayfair.Core.Code.Temporary
{
    using UnityEngine;

    [System.Serializable]
    public class CameraSettingsData
    {
        [System.Serializable]
        public class Container
        {
            [SerializeField]
            private float gestureThreshold = 10f;
            [SerializeField]
            private bool invertAxis = false;
            [SerializeField]
            private float speed = 0.3f;

            public float GestureThreshold { get { return this.gestureThreshold; } }
            public bool InvertAxis { get { return this.invertAxis; } }
            public float Speed { get { return this.speed; } }

            public Container(float gestureThreshold, float speed, bool invertAxis)
            {
                this.gestureThreshold = gestureThreshold;
                this.speed = speed;
                this.invertAxis = invertAxis;
            }
        }

        [Space]
        [SerializeField]
        private Container pitchData = new Container(10f, 0.3f, true);
        [Space]
        [SerializeField]
        private Container yawData = new Container(1, 1.5f, false);
        [Space]
        [SerializeField]
        private Container zoomData = new Container(0.01f, 0.05f, true);

        [Header("Pan")]
        [SerializeField]
        private float panThreshold = 6f;
        [SerializeField]
        private float camPanningSpeed = 1f;

        public Container Pitch { get { return this.pitchData; } }
        public Container Yaw { get { return this.yawData; } }
        public Container Zoom { get { return this.zoomData; } }

        public float CamPanningSpeed { get { return this.camPanningSpeed; } }
        public float PanThreshold { get { return this.panThreshold; } }
    }
}
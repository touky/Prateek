namespace Mayfair.Core.Code.Temporary
{
    using System;

    [Serializable]
    public struct CameraConstraints// : IDatabaseData
    {
        //[Include]
        public float zoomIn;
        //[Include]
        public float zoomOut;
        //[Include]
        public float minPitch;
        //[Include]
        public float maxPitch;

        public CameraConstraints(float zoomIn, float zoomOut, float minPitch, float maxPitch)
        {
            this.zoomIn = zoomIn;
            this.zoomOut = zoomOut;
            this.minPitch = minPitch;
            this.maxPitch = maxPitch;
        }
    }
}
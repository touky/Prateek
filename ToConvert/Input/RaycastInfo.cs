namespace Assets.Prateek.ToConvert.Input
{
    using UnityEngine;

    public struct RaycastInfo
    {
        #region Fields
        private Ray ray;
        internal RaycastHit[] hits;
        #endregion

        #region Properties
        public Ray Ray
        {
            get { return ray; }
            internal set { ray = value; }
        }

        public RaycastHit[] Hits
        {
            get { return hits; }
        }

        public RaycastHit this[LayerMask layer]
        {
            get { return hits[layer]; }
            internal set { hits[layer] = value; }
        }
        #endregion

        #region Constructors
        internal RaycastInfo(int layersCount)
        {
            ray = new Ray();
            hits = new RaycastHit[layersCount];
        }
        #endregion
    }
}

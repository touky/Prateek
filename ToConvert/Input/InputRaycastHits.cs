namespace Assets.Prateek.ToConvert.Input
{
    using System;
    using UnityEngine;

    public class InputRaycastHits
    {
        #region Fields
        private RaycastHit[] emptyHits;
        private RaycastInfo[] raycastInfosPerInput;
        #endregion

        #region Properties
        public RaycastInfo this[InputRaycast inputRaycast]
        {
            get { return raycastInfosPerInput[(int) inputRaycast]; }
            internal set { raycastInfosPerInput[(int) inputRaycast] = value; }
        }
        #endregion

        #region Constructors
        internal InputRaycastHits(int inputCount, int layersCount)
        {
            emptyHits = new RaycastHit[layersCount];
            raycastInfosPerInput = new RaycastInfo[inputCount];
            for (var i = 0; i < raycastInfosPerInput.Length; i++)
            {
                var infos = raycastInfosPerInput[i];
                infos = new RaycastInfo(layersCount);
                raycastInfosPerInput[i] = infos;
            }
        }
        #endregion

        #region Class Methods
        internal void Reset()
        {
            for (var i = 0; i < raycastInfosPerInput.Length; i++)
            {
                var infos = raycastInfosPerInput[i];
                Array.Copy(emptyHits, infos.hits, emptyHits.Length);
            }
        }
        #endregion
    }
}

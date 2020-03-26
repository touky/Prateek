namespace Assets.Prateek.ToConvert.UniqueId
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("UniqueId:{uniqueId}/{GetHexHashCode()}")]
    [Serializable]
    public struct SerializableUniqueId
    {
        #region Settings
        [SerializeField]
        private string uniqueId;
        #endregion

        #region Fields
        private bool isInit;
        private UniqueId internalUniqueId;
        #endregion

        #region Constructors
        public SerializableUniqueId(string uniqueId)
        {
            isInit = false;
            this.uniqueId = uniqueId;
            internalUniqueId = string.Empty;
        }
        #endregion

        #region Class Methods
        public static implicit operator SerializableUniqueId(string uniqueId)
        {
            return new SerializableUniqueId(uniqueId);
        }

        public static implicit operator UniqueId(SerializableUniqueId uniqueId)
        {
            uniqueId.CheckInit();
            return uniqueId.internalUniqueId;
        }

        private void CheckInit()
        {
            if (isInit)
            {
                return;
            }

            isInit = true;
            internalUniqueId = uniqueId;
        }
        #endregion
    }
}

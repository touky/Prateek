namespace Mayfair.Core.Code.Utils.Types.UniqueId
{
    using System;
    using System.Diagnostics;
    using UnityEngine;

    [DebuggerDisplay("Keyname:{keyname}/{GetHexHashCode()}")]
    [Serializable]
    public struct SerializableKeyname
    {
        #region Settings
        [SerializeField]
        private string keyname;
        #endregion

        #region Fields
        private bool initDone;
        private Keyname internalKeyname;
        #endregion

        #region Constructors
        public SerializableKeyname(string keyname)
        {
            initDone = false;
            this.keyname = keyname;
            internalKeyname = string.Empty;
        }
        #endregion

        #region Class Methods
        public static implicit operator SerializableKeyname(string keyname)
        {
            return new SerializableKeyname(keyname);
        }

        public static implicit operator Keyname(SerializableKeyname keyname)
        {
            keyname.CheckInit();
            return keyname.internalKeyname;
        }

        private void CheckInit()
        {
            if (initDone)
            {
                return;
            }

            initDone = true;
            internalKeyname = keyname;
        }

        public override string ToString()
        {
            return keyname;
        }
        #endregion
    }
}

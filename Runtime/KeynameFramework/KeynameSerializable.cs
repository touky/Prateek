namespace Prateek.Runtime.KeynameFramework
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine;

    [DebuggerDisplay("{DebugDisplay,nq}")]
    [Serializable]
    public struct KeynameSerializable
    {
        #region Settings
        [SerializeField]
        private string savedKeyname;
        #endregion

        #region Fields
        private bool initDone;
        private Keyname internalKeyname;
        #endregion

        #region Properties
        private string DebugDisplay { get { return $"Keyname: {ToString()}/{savedKeyname.GetHashCode().ToHex()}"; } }
        #endregion

        #region Constructors
        public KeynameSerializable(string savedKeyname)
        {
            initDone = false;
            this.savedKeyname = savedKeyname;
            internalKeyname = string.Empty;
        }
        #endregion

        #region Class Methods
        public static implicit operator KeynameSerializable(string keyname)
        {
            return new KeynameSerializable(keyname);
        }

        public static implicit operator Keyname(KeynameSerializable keyname)
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
            internalKeyname = savedKeyname;
        }

        public override string ToString()
        {
            return savedKeyname;
        }
        #endregion
    }
}

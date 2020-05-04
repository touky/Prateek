namespace Mayfair.Core.Code.Utils
{
    using UnityEngine;

    public abstract class SystemProperty
    {
        #region Settings
        [SerializeField]
        private string name = string.Empty;
        #endregion

        #region Fields
        protected int nameID = -1;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public int ID
        {
            get
            {
                Init();
                return nameID;
            }
            set { nameID = value; }
        }
        #endregion

        #region Constructors
        protected SystemProperty(string property)
        {
            name = property;
            nameID = -1;
        }
        #endregion

        #region Class Methods
        protected abstract void Init();
        #endregion
    }
}

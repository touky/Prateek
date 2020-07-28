namespace Mayfair.Core.Code.FrameworkSettings
{
    using UnityEngine;

    /// <summary>
    ///     Base class to derive from to create internal settings for you own systems
    /// </summary>
    public abstract class FrameworkSettings<TSingleton, TData, TResource> : FrameworkSettings
        where TSingleton : FrameworkSettings, new()
        where TData : FrameworkSettingsData, new()
        where TResource : FrameworkSettingsResource<TData>
    {
        #region Static and Constants
        private static TSingleton instance;
        #endregion

        #region Fields
        protected TData data = null;
        protected TResource resource = null;
        #endregion

        #region Properties
        public TSingleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new TSingleton();
                    instance.InternalInit();
                }

                return instance;
            }
        }

        public override bool IsAvailable
        {
            get { return data != null; }
        }

        public TData Data
        {
            get { return data; }
        }

        protected virtual string DataPath
        {
            get { return string.Empty; }
        }
        #endregion

        #region Class Methods
        protected override void Init()
        {
            if (data == null && LoadResource(DataPath))
            {
                data = resource.Data;
            }
            else
            {
                data = new TData();
            }
        }

        protected bool LoadResource(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            resource = Resources.Load<TResource>(path);
            if (resource == null)
            {
                Debug.LogError($"Couldn't load settings for {nameof(TSingleton)} at path {path}, using default value.");
                return false;
            }

            return true;
        }
        #endregion
    }
}

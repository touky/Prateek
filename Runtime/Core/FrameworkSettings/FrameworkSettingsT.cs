namespace Prateek.Runtime.Core.FrameworkSettings
{
    using System;
    using UnityEngine;

    /// <summary>
    ///     Base class to derive from to create internal settings for you own systems
    /// </summary>
    [Serializable]
    public abstract class FrameworkSettings<TSingleton, TData, TResource> : FrameworkSettings
        where TSingleton : FrameworkSettings, new()
        where TData : FrameworkSettingsData, new()
        where TResource : FrameworkSettingsResource<TData>
    {
        #region Static and Constants
        private static TSingleton defaultInstance;
        #endregion

        #region Settings
        [SerializeField]
        private string settingsPath = string.Empty;
        #endregion

        #region Fields
        protected TData data = null;
        protected TResource resource = null;
        #endregion

        #region Properties
        public static TSingleton Default
        {
            get
            {
                if (defaultInstance == null)
                {
                    defaultInstance = new TSingleton();
                    defaultInstance.InternalInit();
                }

                return defaultInstance;
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

        protected virtual string DefaultPath
        {
            get { return string.Empty; }
        }
        #endregion

        #region Class Methods
        protected override void Init()
        {
            var path = string.IsNullOrEmpty(settingsPath) ? DefaultPath : settingsPath;
            if (data == null && LoadResource(path))
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

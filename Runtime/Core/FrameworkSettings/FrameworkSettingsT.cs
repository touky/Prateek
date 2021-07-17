namespace Prateek.Runtime.Core.FrameworkSettings
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
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

        public override Type ResourceType { get { return typeof(TResource); } }

        public TData Data
        {
            get { return data; }
        }
        #endregion

        #region Class Methods
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainReload()
        {
            defaultInstance = null;
        }

        protected override void Init()
        {
            var path = DefaultPath;
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
                Debug.LogWarning($"Couldn't load settings for {typeof(TSingleton).Name} at path {path}, using default value.");
                return false;
            }

            return true;
        }
        #endregion
    }
}

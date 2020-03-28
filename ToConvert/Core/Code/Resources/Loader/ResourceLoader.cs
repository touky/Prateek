namespace Mayfair.Core.Code.Resources.Loader
{
    using System;
    using System.Diagnostics;
    using Mayfair.Core.Code.Resources.Enums;
    using Mayfair.Core.Code.Resources.ResourceTree;
    using Mayfair.Core.Code.Utils.Debug;

    [DebuggerDisplay("{resource.ToString()}, Location: {location}, Status: {status}")]
    public abstract class ResourceLoader : IResourceLoader, ITreeLeafLocator
    {
        #region LoaderBehaviour enum
        internal enum LoaderBehaviour
        {
            Nothing,
            Asset,
            Scene
        }
        #endregion

        #region Fields
        //benjaminh: I'm not a fan of boxing this, but this is not an operation done too much time, and it's tied to an important system, so ok for just this.
        protected object resource;
        protected string location;
        protected AsyncStatus status = AsyncStatus.Nothing;
        protected Action loadCompletedAction;

        private LoaderBehaviour behaviour = LoaderBehaviour.Nothing;
        private int refCount = 0;
        #endregion

        #region Properties
        internal Action LoadCompleted
        {
            set { loadCompletedAction = value; }
        }

        internal LoaderBehaviour Behaviour
        {
            get { return behaviour; }
            set { behaviour = value; }
        }

        /// <summary>
        ///     Keeps track of the amount of active instances for this object
        ///     When RefCount is above zero, Automatically loads the asset
        ///     When InstanceCount falls to zero, Automatically unloads the asset
        /// </summary>
        internal int RefCount
        {
            get { return refCount; }
            set
            {
                bool hasRefs = refCount != 0;

                refCount = value;

                if (refCount < 0)
                {
                    throw new Exception($"refCount for {location} shouldn't be negative.");
                }

                if (!hasRefs && refCount != 0)
                {
                    Load();
                }
                else if (hasRefs && refCount == 0)
                {
                    Unload();
                }
            }
        }
        #endregion

        #region Constructors
        private ResourceLoader() { }

        protected ResourceLoader(string location)
        {
            this.location = location;
        }
        #endregion

        #region Class Methods
        private void Load()
        {
            switch (behaviour)
            {
                case LoaderBehaviour.Asset:
                {
                    LoadAsset();
                    break;
                }
                case LoaderBehaviour.Scene:
                {
                    LoadSceneAsync();
                    break;
                }
                default:
                {
                    throw new Exception("Undefined resource loader behaviour");
                }
            }
        }

        private void Unload()
        {
            switch (behaviour)
            {
                case LoaderBehaviour.Asset:
                {
                    UnloadAsset();
                    break;
                }
                case LoaderBehaviour.Scene:
                {
                    UnloadSceneAsync();
                    break;
                }
                default:
                {
                    throw new Exception("Undefined resource loader behaviour");
                }
            }
        }

        protected abstract void LoadAsset();
        protected abstract void UnloadAsset();

        protected abstract void LoadSceneAsync();
        protected abstract void UnloadSceneAsync();

        internal bool CanConvert<T>()
        {
            return resource is T;
        }

        internal T ConvertResource<T>()
        {
            if (CanConvert<T>())
            {
                return (T) resource;
            }

            return default;
        }

        protected void OnLoadCompleted()
        {
            if (loadCompletedAction != null)
            {
                loadCompletedAction.Invoke();
            }
        }
        #endregion

        #region IResourceLoader Members
        public bool IsDone
        {
            get { return status != AsyncStatus.Nothing && status != AsyncStatus.Loading; }
        }

        public AsyncStatus Status
        {
            get { return status; }
        }

        public virtual float PercentComplete
        {
            get
            {
                switch (status)
                {
                    case AsyncStatus.Loading: { return 0.5f; }
                    case AsyncStatus.Loaded:  { return 1; }
                }

                return 0;
            }
        }

        public string Location
        {
            get { return location; }
        }
        #endregion
    }
}

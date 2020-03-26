namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.resource.ToString()}, Location: {loader.location}")]
    public abstract class AbstractResourceReference<TResourceType, TResourceReference> : IInstanceCount, IAbstractResourceReference
        where TResourceReference : AbstractResourceReference<TResourceType, TResourceReference>
    {
        #region Fields
        private bool hasMarkedLoadRef = false;
        private int instanceCount = 0;
        protected ResourceLoader loader;
        protected TResourceType resource;
        protected Action<TResourceReference> asyncCompletedAction;
        #endregion

        #region Properties
        protected TResourceType TypedResource
        {
            get { return resource; }
        }
        #endregion

        #region Constructors
        protected AbstractResourceReference(ResourceLoader loader)
        {
            this.loader = loader;
        }
        #endregion

        #region Class Methods
        protected void InternalLoad()
        {
            if (!hasMarkedLoadRef)
            {
                hasMarkedLoadRef = true;
                loader.RefCount++;
            }
        }

        protected void InternalUnload()
        {
            if (instanceCount > 0)
            {
                throw new Exception($"Resource reference instance for {loader.Location} were not destroyed properly !");
            }

            if (hasMarkedLoadRef)
            {
                hasMarkedLoadRef = false;
                loader.RefCount--;
            }
        }

        protected void OnAsyncCompleted()
        {
            RefreshResource();

            if (asyncCompletedAction != null)
            {
                asyncCompletedAction.Invoke(this as TResourceReference);
            }
        }

        protected virtual void RefreshResource()
        {
            if (loader.CanConvert<TResourceType>())
            {
                resource = loader.ConvertResource<TResourceType>();
            }
        }
        #endregion

        #region IAbstractResourceReference Members
        public IResourceLoader Loader
        {
            get { return loader; }
        }

        public Action<IAbstractResourceReference> LoadCompleted
        {
            set { asyncCompletedAction = value; }
        }

        public abstract void LoadAsync();
        #endregion

        #region IInstanceCount Members
        public void IncrementInstanceCount()
        {
            instanceCount++;
            loader.RefCount++;
        }

        public void DecrementInstanceCount()
        {
            instanceCount--;
            loader.RefCount--;

            if (instanceCount == 0)
            {
                InternalUnload();
            }
        }
        #endregion

        ~AbstractResourceReference()
        {
            InternalUnload();
        }
    }
}

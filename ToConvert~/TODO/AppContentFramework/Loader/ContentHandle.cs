namespace Mayfair.Core.Code.Resources.Loader
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("{typeof(TResourceType).Name}, {loader.content.ToString()}, Location: {loader.location}")]
    public abstract class ContentHandle<TContentType, TContentHandle> : IInstanceRef, IContentHandle
        where TContentHandle : ContentHandle<TContentType, TContentHandle>
    {
        #region Fields
        private bool hasMarkedLoadRef = false;
        private int instanceCount = 0;
        protected ContentLoader loader;
        protected TContentType content;
        protected Action<TContentHandle> asyncCompletedAction;
        #endregion

        #region Properties
        protected TContentType TypedContent
        {
            get { return content; }
        }
        #endregion

        #region Constructors
        protected ContentHandle(ContentLoader loader)
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
                asyncCompletedAction.Invoke(this as TContentHandle);
            }
        }

        protected virtual void RefreshResource()
        {
            if (loader.CanConvert<TContentType>())
            {
                content = loader.ConvertResource<TContentType>();
            }
        }
        #endregion

        #region IAbstractResourceReference Members
        public IContentLoader Loader
        {
            get { return loader; }
        }

        public Action<IContentHandle> LoadCompleted
        {
            set { asyncCompletedAction = value; }
        }

        public abstract void LoadAsync();
        #endregion

        #region IInstanceCount Members
        public void IncrementInstanceRef()
        {
            instanceCount++;
            loader.RefCount++;
        }

        public void DecrementInstanceRef()
        {
            instanceCount--;
            loader.RefCount--;

            if (instanceCount == 0)
            {
                InternalUnload();
            }
        }
        #endregion

        ~ContentHandle()
        {
            InternalUnload();
        }
    }
}

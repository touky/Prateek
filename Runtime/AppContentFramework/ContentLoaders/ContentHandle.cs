namespace Prateek.Runtime.AppContentFramework.ContentLoaders
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine.Assertions;

    [DebuggerDisplay("{typeof(TContentType).Name}, {loader.content.ToString()}, Location: {loader.path}")]
    public abstract class ContentHandle<TContentType, TContentHandle>
        : IContentHandle
        where TContentHandle : ContentHandle<TContentType, TContentHandle>
    {
        #region Static and Constants
        private const int NO_INSTANCE = 0;
        #endregion

        #region Fields
        private int instanceCount = Const.INDEX_NONE;
        private bool autoUnload = true;

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
        protected ContentHandle(ContentLoader loader, bool autoUnload = true)
        {
            this.autoUnload = autoUnload;
            this.loader = loader;
            this.loader.LoadCompleted = OnAsyncCompleted;
        }

        ~ContentHandle()
        {
            Assert.IsTrue(instanceCount == Const.INDEX_NONE || autoUnload, $"{GetType().Name} was not unloaded before destroying");

            Unload();
        }
        #endregion

        #region Class Methods
        protected void OnAsyncCompleted()
        {
            RetrieveContent();

            asyncCompletedAction.SafeInvoke(this as TContentHandle);
        }

        protected virtual void RetrieveContent()
        {
            content = default;
            if (loader.IsContentValid<TContentType>())
            {
                content = loader.GetContent<TContentType>();
            }
        }

        public TContentHandle As<TContentHandle>()
            where TContentHandle : class, IContentHandle
        {
            return this as TContentHandle;
        }
        #endregion

        #region IContentHandle Members
        public bool HasReferences
        {
            get { return instanceCount > 0; }
        }

        public IContentLoader Loader
        {
            get { return loader; }
        }

        public Action<IContentHandle> LoadCompleted
        {
            set { asyncCompletedAction = value; }
        }
        #endregion

        public void Load()
        {
            if (instanceCount > Const.INDEX_NONE)
            {
                return;
            }

            content = default;

            IncrementReferences();
        }

        public void Unload()
        {
            Assert.IsFalse(autoUnload, $"{GetType().Name} is trying to unload manually when it is setup to do it automatically");
            
            content = default;

            while (instanceCount > Const.INDEX_NONE)
            {
                DecrementReferences();
            }
        }

        #region IInstanceRef Members
        public void IncrementReferences()
        {
            instanceCount++;
            loader.RefCount++;
        }

        public void DecrementReferences()
        {
            if (instanceCount <= Const.INDEX_NONE)
            {
                return;
            }

            instanceCount--;
            loader.RefCount--;

            if (instanceCount == NO_INSTANCE && autoUnload)
            {
                instanceCount--;
                loader.RefCount--;
            }
        }
        #endregion
    }
}

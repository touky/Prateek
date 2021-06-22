namespace Prateek.Runtime.AppContentFramework.ContentLoaders
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;
    using UnityEngine.Assertions;

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
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

        private ContentLoader loader;
        private TContentType content;
        private Action<TContentHandle> asyncCompletedAction;
        #endregion

        #region Properties
        /// <summary>
        /// Child classes must use this property to access the content once loaded
        /// This is deliberately not public to give the possibility to tinker with the content
        /// (See GameObject & Component instance count management)
        /// </summary>
        protected TContentType TypedContent
        {
            get { return content; }
        }

        private string DebuggerDisplay { get { return $"{DebuggerDisplayType}, {DebuggerDisplayFile}"; } }

        protected string DebuggerDisplayType
        {
            get { return $"{typeof(TContentHandle).Name}<{typeof(TContentType).Name}>"; }
        }
        
        protected string DebuggerDisplayFile
        {
            get { return $"{(content != null ? content.ToString() : "Not loaded")}, Location: {loader.Path}"; }
        }
        #endregion

        #region Constructors
        protected ContentHandle()
            : this(true)
        { }

        protected ContentHandle(bool autoUnload)
        {
            this.autoUnload = autoUnload;
        }

        ~ContentHandle()
        {
            Assert.IsTrue(instanceCount == Const.INDEX_NONE || autoUnload, $"{GetType().Name} was not unloaded before destroying");

            Unload();
        }
        #endregion

        #region Class Methods
        public virtual void Init(ContentLoader loader)
        {
            this.loader = loader;
            this.loader.LoadCompleted = OnAsyncCompleted;
        }

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

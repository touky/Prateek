namespace Prateek.Runtime.AppContentFramework.ContentLoaders
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Interfaces.IDebuggerDisplay;
    using UnityEngine.Assertions;

    [DebuggerDisplay(ConstDebuggerDisplay.Key)]
    public abstract class ContentHandle<TContent>
        : IContentHandleInit
        , IContentHandle
        , IDebuggerDisplay
    {
        #region Static and Constants
        private const int NO_INSTANCE = 0;
        #endregion

        #region Fields
        private int instanceCount = Const.INDEX_NONE;
        private bool autoUnload = true;

        private ContentLoader loader;
        private Action<IContentHandle> asyncCompletedAction;
        #endregion

        #region Properties
        protected TContent TypedContent
        {
            get
            {
                if (loader.IsContentValid<TContent>())
                {
                    return loader.GetContent<TContent>();
                }

                return default;
            }
        }
        #endregion

        #region DebuggerDisplay
        public string DebuggerDisplay { get { return $"{DebuggerDisplayType}, {DebuggerDisplayFile}"; } }

        protected string DebuggerDisplayType
        {
            get { return $"{GetType().Name}<{(loader != null ? loader.GetType().Name : "-NO LOADER-")}>"; }
        }
        
        protected string DebuggerDisplayFile
        {
            get { return $"[Inst{instanceCount}]{(autoUnload ? string.Empty : "[MANUAL-UNLOAD]")} {loader.DebuggerDisplay}"; }
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
        void IContentHandleInit.Init(ContentLoader loader)
        {
            this.loader = loader;
            this.loader.LoadCompleted = OnAsyncCompleted;

            OnInit(loader);
        }

        protected virtual void OnInit(ContentLoader loader)
        {
        }

        protected void OnAsyncCompleted()
        {
            asyncCompletedAction.SafeInvoke(this);
        }

        public TContentHandle As<TContentHandle>()
            where TContentHandle : class, IContentHandle
        {
            return this as TContentHandle;
        }

        public void Load()
        {
            if (instanceCount > Const.INDEX_NONE)
            {
                return;
            }

            IncrementReferences();
        }

        public void Unload()
        {
            Assert.IsFalse(autoUnload, $"{GetType().Name} is trying to unload manually when it is setup to do it automatically");

            while (instanceCount > Const.INDEX_NONE)
            {
                DecrementReferences();
            }
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

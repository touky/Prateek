namespace Prateek.Runtime.AppContentFramework.ContentLoaders
{
    using System;
    using System.Diagnostics;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    [DebuggerDisplay("{content.ToString()}, Location: {path}, Status: {status}")]
    public abstract class ContentLoader : IContentLoader, IHierarchicalTreeLeaf
    {
        #region Fields
        /// <summary>
        /// Content is an object because all loaded content will necessarly be classes
        /// and I didn't want to make this class a generic one
        /// </summary>
        protected object content;
        protected string path;

        private LoaderParameters parameters;
        private int refCount = 0;

        protected ContentAsyncStatus status = ContentAsyncStatus.Nothing;
        protected Action actionOnLoadComplete;
        #endregion

        #region Properties
        public Action LoadCompleted
        {
            set { actionOnLoadComplete = value; }
        }

        /// <summary>
        ///     Keeps track of the amount of user that reference that asset
        ///     On RefCount > 0: Automatically loads the asset
        ///     On RefCount == 0: Automatically unloads the asset
        /// </summary>
        internal int RefCount
        {
            get { return refCount; }
            set
            {
                var hasRefs = refCount != 0;

                refCount = value;

                if (refCount < 0)
                {
                    throw new Exception($"refCount for {path} shouldn't be negative.");
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
        private ContentLoader() { }

        protected ContentLoader(string path)
        {
            this.path = path;
        }
        #endregion

        #region Class Methods
        public void SetLoadContextParameters(LoaderParameters parameters)
        {
            this.parameters = parameters;
        }

        protected TExpectedType ValidateParameterType<TExpectedType>(LoaderParameters parameters)
            where TExpectedType : LoaderParameters
        {
            if (!(parameters is TExpectedType aParameters))
            {
                throw new Exception($"{nameof(LoaderParameters)} are of the wrong type: {parameters.GetType().Name}");
            }

            return aParameters;
        }

        private void Load()
        {
            if (parameters == null)
            {
                throw new Exception($"Undefined context parameters for {path} loading");
            }

            Load(parameters);
        }

        private void Unload()
        {
            if (parameters == null)
            {
                throw new Exception($"Undefined context parameters for {path} unloading");
            }

            Unload(parameters);
        }

        internal bool IsContentValid<T>()
        {
            return content is T;
        }

        internal T GetContent<T>()
        {
            if (IsContentValid<T>())
            {
                return (T) content;
            }

            return default;
        }

        protected void OnLoadCompleted()
        {
            actionOnLoadComplete.SafeInvoke();
        }

        protected abstract void Load(LoaderParameters parameters);
        protected abstract void Unload(LoaderParameters parameters);
        #endregion

        #region IContentLoader Members
        public bool HasFinishedLoading
        {
            get { return status != ContentAsyncStatus.Nothing && status != ContentAsyncStatus.Loading; }
        }

        public ContentAsyncStatus Status
        {
            get { return status; }
        }

        public virtual float PercentComplete
        {
            get
            {
                switch (status)
                {
                    case ContentAsyncStatus.Loading: { return 0.5f; }
                    case ContentAsyncStatus.Loaded:  { return 1; }
                }

                return 0;
            }
        }

        public string Path
        {
            get { return path; }
        }
        #endregion
    }
}

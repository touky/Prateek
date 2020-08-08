namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.Core;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class ContentAccessChangedResponse<TResourceRef, TResourceType>
        : ContentAccessChangedResponse
        where TResourceRef : ContentHandle<TResourceType, TResourceRef>
    {
        #region Properties
        public DiffList<TResourceRef> content
        {
            get { return (request.storage as StorageDiff<TResourceRef>).content; }
        }
        #endregion

        #region Class Methods
        public override void Add(IHierarchicalTreeLeaf leaf)
        {
            if (request.storage == null)
            {
                request.storage = new StorageDiff<TResourceRef>();
            }

            var storage = request.storage as StorageDiff<TResourceRef>;
            var content = leaf as ContentLoader;
            if (content != null)
            {
                storage.content.Add(GetResourceRef(content));
            }
        }

        protected abstract TResourceRef GetResourceRef(ContentLoader loader);
        #endregion

        #region Nested type: StorageDiff
        internal class StorageDiff<T> : Storage
        {
            #region Fields
            public DiffList<T> content;
            #endregion
        }
        #endregion
    }
}

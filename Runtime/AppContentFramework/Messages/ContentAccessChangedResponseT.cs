namespace Prateek.Runtime.AppContentFramework.Messages
{
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.Core;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class ContentAccessChangedResponse<TContentHandle, TContentType>
        : ContentAccessChangedResponse
        where TContentHandle : ContentHandle<TContentType, TContentHandle>
    {
        #region Properties
        public DiffList<TContentHandle> Content
        {
            get { return (request.storage as StorageDiff<TContentHandle>).content; }
        }
        #endregion

        #region Class Methods
        public override void Add(IHierarchicalTreeLeaf leaf)
        {
            if (request.storage == null)
            {
                request.storage = new StorageDiff<TContentHandle>();
            }

            var storage = request.storage as StorageDiff<TContentHandle>;
            var content = leaf as ContentLoader;
            if (content != null)
            {
                storage.content.Add(GetHandle(content));
            }
        }

        protected abstract TContentHandle GetHandle(ContentLoader loader);
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

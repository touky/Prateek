namespace Prateek.Runtime.AppContentFramework.Messages
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces;
    using Prateek.Runtime.Core;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class ContentAccessChangedResponse<TContentHandle>
        : ContentAccessChangedResponse
        where TContentHandle : IContentHandle, new()
    {
        #region Properties
        public DiffList<TContentHandle> Content
        {
            get { return request.storage != null
                    ? (request.storage as StorageDiff<TContentHandle>).content
                    : default; }
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
                var handle = GetHandle(content);
                storage.content.Add(handle);
            }
        }

        protected TContentHandle GetHandle(ContentLoader loader)
        {
            var handle = GetHandle();
            if (handle is IContentHandleInit init)
            {
                init.Init(loader);
            }
            return handle;
        }

        protected virtual TContentHandle GetHandle()
        {
            return new TContentHandle();
        }
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

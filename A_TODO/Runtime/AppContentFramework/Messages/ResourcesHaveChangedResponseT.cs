namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public abstract class ResourcesHaveChangedResponse<TResourceRef, TResourceType> : ResourcesHaveChangedResponse
        where TResourceRef : ContentHandle<TResourceType, TResourceRef>
    {
        #region Fields
        private List<TResourceRef> references = new List<TResourceRef>();
        #endregion

        #region Properties
        public List<TResourceRef> References
        {
            get { return this.references; }
        }
        #endregion

        #region ITreeIdentificationResult
        public override void Add(IHierarchicalTreeLeaf leaf)
        {
            ContentLoader content = leaf as ContentLoader;
            if (content != null)
            {
                this.references.Add(GetResourceRef(content));
            }
        }

        protected abstract TResourceRef GetResourceRef(ContentLoader loader);
        #endregion
    }
}

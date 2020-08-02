namespace Prateek.A_TODO.Runtime.AppContentFramework.Messages
{
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.A_TODO.Runtime.AppContentFramework.ResourceTree;

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
        public override bool Match(ITreeLeafLocator leafLocator)
        {
            return true;
        }

        public override void Add(ITreeLeafLocator leafLocator)
        {
            ContentLoader content = leafLocator as ContentLoader;
            if (content != null)
            {
                this.references.Add(GetResourceRef(content));
            }
        }

        protected abstract TResourceRef GetResourceRef(ContentLoader loader);
        #endregion
    }
}

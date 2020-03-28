namespace Mayfair.Core.Code.Resources.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.Resources.ResourceTree;

    public abstract class ResourcesHaveChangedResponse<TResourceRef, TResourceType> : ResourcesHaveChangedResponse
        where TResourceRef : AbstractResourceReference<TResourceType, TResourceRef>
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
            ResourceLoader resource = leafLocator as ResourceLoader;
            if (resource != null)
            {
                this.references.Add(GetResourceRef(resource));
            }
        }

        protected abstract TResourceRef GetResourceRef(ResourceLoader loader);
        #endregion
    }
}

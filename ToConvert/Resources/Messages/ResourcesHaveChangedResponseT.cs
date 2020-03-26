namespace Assets.Prateek.ToConvert.Resources.Messages
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Resources.Loader;
    using Assets.Prateek.ToConvert.Resources.ResourceTree;

    public abstract class ResourcesHaveChangedResponse<TResourceRef, TResourceType> : ResourcesHaveChangedResponse
        where TResourceRef : AbstractResourceReference<TResourceType, TResourceRef>
    {
        #region Fields
        private List<TResourceRef> references = new List<TResourceRef>();
        #endregion

        #region Properties
        public List<TResourceRef> References
        {
            get { return references; }
        }
        #endregion

        #region ITreeIdentificationResult
        public override bool Match(ITreeLeafLocator leafLocator)
        {
            return true;
        }

        public override void Add(ITreeLeafLocator leafLocator)
        {
            var resource = leafLocator as ResourceLoader;
            if (resource != null)
            {
                references.Add(GetResourceRef(resource));
            }
        }

        protected abstract TResourceRef GetResourceRef(ResourceLoader loader);
        #endregion
    }
}

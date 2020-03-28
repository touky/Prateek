namespace Mayfair.Core.Code.Resources.Messages
{
    using Mayfair.Core.Code.Resources.Loader;
    using UnityEngine.ResourceManagement.ResourceProviders;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged message
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class ScenesHaveChanged : ResourcesHaveChangedResponse<SceneReference, SceneInstance>
    {
        #region Class Methods
        #region ITreeIdentificationResult
        protected override SceneReference GetResourceRef(ResourceLoader loader)
        {
            return new SceneReference(loader);
        }
        #endregion
        #endregion
    }
}

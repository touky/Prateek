namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using UnityEngine.ResourceManagement.ResourceProviders;

    /// <summary>
    ///     Use this class as base to implement your own callback for the ResourcesHaveChanged notice
    /// </summary>
    /// <typeparam name="TResourceType">The type of the resource</typeparam>
    public abstract class ScenesHaveChangedResponse : ContentAccessChangedResponse<SceneReference, SceneInstance>
    {
        #region Class Methods
        #region ITreeIdentificationResult
        protected override SceneReference GetResourceRef(ContentLoader loader)
        {
            return new SceneReference(loader);
        }
        #endregion
        #endregion
    }
}

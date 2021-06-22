namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.AppContentFramework.Unity.Handles;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public abstract class SceneContentAccessResponse
        : ContentAccessChangedResponse<SceneInstance, SceneHandle>
    {
        #region Class Methods
        protected override SceneHandle GetHandle()
        {
            return new SceneHandle();
        }
        #endregion
    }
}

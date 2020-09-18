namespace Prateek.Runtime.AppContentFramework.Unity.Commands
{
    using Prateek.Runtime.AppContentFramework.Messages;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class SceneContentAccessRequest
        : ContentAccessRequest<SceneInstance>
    {
        #region Class Methods
        protected override bool ValidateResponse()
        {
            return holder.Validate<SceneContentAccessResponse>();
        }
        #endregion
    }
}

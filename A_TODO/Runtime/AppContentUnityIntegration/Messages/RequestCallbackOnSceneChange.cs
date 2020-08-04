namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using UnityEngine.ResourceManagement.ResourceProviders;

    /// <summary>
    ///     Use this class as base to receive callback for any resource that have been changed
    /// </summary>
    /// <typeparam name="TChangeMessage">The ResourcesHaveChanged type notice to use as callback</typeparam>
    /// <typeparam name="TResourceType">The resource type of your data</typeparam>
    //todo public class RequestCallbackOnSceneChange<TChangeMessage> : RequestAccessToContent<SceneInstance>
    //todo     where TChangeMessage : ScenesHaveChanged, new()
    //todo {
    //todo     #region Class Methods
    //todo     protected override ResponseCommand CreateNewResponse()
    //todo     {
    //todo         return Create<TChangeMessage>();
    //todo     }
    //todo     #endregion
    //todo }
}

namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to receive callback for any resource that have been changed
    /// </summary>
    /// <typeparam name="TChangeMessage">The ResourcesHaveChanged type notice to use as callback</typeparam>
    /// <typeparam name="TResourceType">The resource type of your data</typeparam>
    public class RequestCallbackOnGameObjectResourceChange<TChangeMessage> : RequestAccessToContent<GameObject>
        where TChangeMessage : GameObjectResourcesHaveChanged, new()
    {
        #region Class Methods
        protected override ResponseCommand CreateNewResponse()
        {
            return Create<TChangeMessage>();
        }
        #endregion
    }
}

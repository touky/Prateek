namespace Mayfair.Core.Code.Resources.Messages
{
    using Prateek.NoticeFramework.Notices.Core;
    using UnityEngine;

    /// <summary>
    ///     Use this class as base to receive callback for any resource that have been changed
    /// </summary>
    /// <typeparam name="TChangeMessage">The ResourcesHaveChanged type notice to use as callback</typeparam>
    /// <typeparam name="TResourceType">The resource type of your data</typeparam>
    public class RequestCallbackOnMonoBehaviourResourceChange<TChangeMessage, TResourceType> : RequestCallbackOnChange<TResourceType>
        where TChangeMessage : MonoBehaviourResourcesHaveChanged<TResourceType>, new()
        where TResourceType : MonoBehaviour
    {
        #region Class Methods
        protected override ResponseNotice CreateNewResponse()
        {
            return Create<TChangeMessage>();
        }
        #endregion
    }
}

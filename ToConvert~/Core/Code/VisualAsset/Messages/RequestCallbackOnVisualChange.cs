namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.Resources.Messages;
    using Prateek.NoticeFramework.Notices.Core;
    using UnityEngine;

    public class RequestCallbackOnVisualChange<TChangeMessage> : RequestAccessToContent<GameObject>
        where TChangeMessage : VisualResourceHasChanged, new()
    {
        #region Class Methods
        protected override ResponseNotice CreateNewResponse()
        {
            return Notice.Create<TChangeMessage>();
        }
        #endregion
    }
}

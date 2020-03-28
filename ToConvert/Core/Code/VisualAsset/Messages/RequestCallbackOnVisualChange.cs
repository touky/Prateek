namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Resources.Messages;
    using UnityEngine;

    public class RequestCallbackOnVisualChange<TChangeMessage> : RequestCallbackOnChange<GameObject>
        where TChangeMessage : VisualResourceHasChanged, new()
    {
        #region Class Methods
        protected override ResponseMessage CreateNewResponse()
        {
            return Message.Create<TChangeMessage>();
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Mayfair.Core.Code.Resources.Messages;
    using Commands.Core;
    using UnityEngine;

    public class RequestCallbackOnVisualChange<TChangeMessage> : RequestAccessToContent<GameObject>
        where TChangeMessage : VisualResourceHasChanged, new()
    {
        #region Class Methods
        protected override ResponseCommand CreateNewResponse()
        {
            return Command.Create<TChangeMessage>();
        }
        #endregion
    }
}

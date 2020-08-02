namespace Mayfair.Core.Code.VisualAsset.Messages
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
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

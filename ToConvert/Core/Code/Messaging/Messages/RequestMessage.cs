namespace Mayfair.Core.Code.Messaging.Messages
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class RequestMessage : TargetedMessage
    {
        #region Class Methods
        public ResponseMessage GetResponse()
        {
            ResponseMessage response = CreateNewResponse();
            response.Init(this);
            return response;
        }

        protected abstract ResponseMessage CreateNewResponse();
        #endregion
    }
}

namespace Mayfair.Core.Code.Messaging.Messages
{
    using System.Diagnostics;
    using Mayfair.Core.Code.Messaging.Communicator;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class RequestMessage<TResponseType> : RequestMessage
        where TResponseType : ResponseMessage, new()
    {
        #region Class Methods
        public new TResponseType GetResponse()
        {
            return base.GetResponse() as TResponseType;
        }

        protected override ResponseMessage CreateNewResponse()
        {
            return Message.Create<TResponseType>();
        }
        #endregion
    }
}


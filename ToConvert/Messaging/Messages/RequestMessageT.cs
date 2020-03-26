namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Diagnostics;

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
            return Create<TResponseType>();
        }
        #endregion
    }
}

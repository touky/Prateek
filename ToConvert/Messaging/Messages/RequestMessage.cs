namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Diagnostics;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class RequestMessage : TargetedMessage
    {
        #region Class Methods
        public ResponseMessage GetResponse()
        {
            var response = CreateNewResponse();
            response.Init(this);
            return response;
        }

        protected abstract ResponseMessage CreateNewResponse();
        #endregion
    }
}

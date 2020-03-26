namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.Messaging.Communicator;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class ResponseMessage : Message
    {
        #region Fields
        private ILightMessageCommunicator recipient = null;
        #endregion

        #region Properties
        public ILightMessageCommunicator Recipient
        {
            get { return recipient; }
        }

        //We allow message type spoofing for Children messages
        public override long MessageID
        {
            get { return ConvertToId(GetType(), recipient); }
        }
        #endregion

        #region Class Methods
        public virtual void Init(RequestMessage request)
        {
            recipient = request.Sender;
        }
        #endregion
    }
}

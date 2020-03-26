namespace Assets.Prateek.ToConvert.Messaging
{
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.Service;

    public abstract class MessageServiceProvider : ServiceProviderBehaviour
    {
        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<MessageService, MessageServiceProvider>(this);
        }

        public abstract void ReceiveMessage(MessageService service, Message receivedMessage);
        #endregion
    }
}

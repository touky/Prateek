namespace Assets.Prateek.ToConvert.Messaging.ServiceProvider
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public sealed class LocalMessageServiceProvider : MessageServiceProvider
    {
        #region Properties
        public override bool IsProviderValid
        {
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Class Methods
        public override void ReceiveMessage(MessageService service, Message receivedMessage)
        {
            service.AddMessage(receivedMessage);
        }
        #endregion
    }
}

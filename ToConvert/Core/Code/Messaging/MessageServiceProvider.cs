namespace Mayfair.Core.Code.Messaging
{
    using Mayfair.Core.Code.Messaging.Messages;

    public abstract class MessageServiceProvider : Service.ServiceProviderBehaviour
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

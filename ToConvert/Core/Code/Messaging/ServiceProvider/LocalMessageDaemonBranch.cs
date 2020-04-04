namespace Mayfair.Core.Code.Messaging.ServiceProvider
{
    using Mayfair.Core.Code.Messaging.Messages;

    public sealed class LocalMessageDaemonBranch : MessageDaemonBranch
    {
        #region Class Methods
        public override void ReceiveMessage(MessageDaemonCore daemonCore, Message receivedMessage)
        {
            daemonCore.AddMessage(receivedMessage);
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.Messaging.ServiceProvider
{
    using Mayfair.Core.Code.Messaging.Messages;

    public sealed class LocalMessageDaemonBranch : MessageDaemonBranch
    {
        #region Properties
        protected override bool IsAliveInternal
        {
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Class Methods
        public override void ReceiveMessage(MessageDaemonCore daemonCore, Message receivedMessage)
        {
            daemonCore.AddMessage(receivedMessage);
        }
        #endregion
    }
}

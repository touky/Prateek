namespace Mayfair.Core.Code.Messaging
{
    using Mayfair.Core.Code.Messaging.Messages;
    using Prateek.DaemonCore.Code.Branches;

    public abstract class MessageDaemonBranch : DaemonBranchBehaviour<MessageDaemonCore, MessageDaemonBranch>
    {
        #region Class Methods
        public abstract void ReceiveMessage(MessageDaemonCore daemonCore, Message receivedMessage);
        #endregion
    }
}

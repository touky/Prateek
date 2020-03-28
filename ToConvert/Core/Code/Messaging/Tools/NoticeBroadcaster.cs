namespace Mayfair.Core.Code.Messaging.Tools
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;

    public struct NoticeBroadcaster<TNotice>
        where TNotice : BroadcastMessage, new()
    {
        #region Fields
        private ILightMessageCommunicator communicator;
        #endregion

        #region Constructors
        public NoticeBroadcaster(ILightMessageCommunicator communicator)
        {
            this.communicator = communicator;
        }
        #endregion

        #region Class Methods
        public void Broadcast()
        {
            TNotice notice = Message.Create<TNotice>();

            this.communicator.Broadcast(notice);
        }
        #endregion
    }
}

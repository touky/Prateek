namespace Assets.Prateek.ToConvert.Messaging.Tools
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;

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
            var notice = Message.Create<TNotice>();

            communicator.Broadcast(notice);
        }
        #endregion
    }
}

namespace Prateek.NoticeFramework.Tools
{
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public struct NoticeBroadcaster<TNotice>
        where TNotice : BroadcastNotice, new()
    {
        #region Fields
        private INoticeTransmitter transmitter;
        #endregion

        #region Constructors
        public NoticeBroadcaster(INoticeTransmitter transmitter)
        {
            this.transmitter = transmitter;
        }
        #endregion

        #region Class Methods
        public void Broadcast()
        {
            TNotice notice = Notice.Create<TNotice>();

            this.transmitter.Broadcast(notice);
        }
        #endregion
    }
}

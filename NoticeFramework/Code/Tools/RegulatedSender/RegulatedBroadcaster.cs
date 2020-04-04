namespace Prateek.NoticeFramework.Tools
{
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class RegulatedBroadcaster<TNotice> : RegulatedNoticeSender<TNotice, INoticeTransmitter>
        where TNotice : BroadcastNotice, new()
    {
        #region Constructors
        public RegulatedBroadcaster(INoticeTransmitter transmitter) : base(transmitter) { }
        public RegulatedBroadcaster(INoticeTransmitter transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Broadcast(nextMessage);
        }
        #endregion
    }
}

namespace Prateek.NoticeFramework.Tools
{
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class RegulatedDirectSender<TNotice> : RegulatedNoticeSender<TNotice, INoticeTransmitter>
        where TNotice : DirectNotice, new()
    {
        #region Constructors
        public RegulatedDirectSender(INoticeTransmitter transmitter) : base(transmitter) { }
        public RegulatedDirectSender(INoticeTransmitter transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Send(nextMessage);
        }
        #endregion
    }
}

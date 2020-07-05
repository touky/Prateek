namespace Prateek.NoticeFramework.Tools
{
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class RegulatedTargetedSender<TNotice> : RegulatedNoticeSender<TNotice, INoticeReceiver>
        where TNotice : TargetedNotice, new()
    {
        #region Constructors
        public RegulatedTargetedSender(INoticeReceiver transmitter) : base(transmitter) { }
        public RegulatedTargetedSender(INoticeReceiver transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Send(nextMessage);
        }
        #endregion
    }
}

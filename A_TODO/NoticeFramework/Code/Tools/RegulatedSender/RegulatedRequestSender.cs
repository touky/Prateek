namespace Prateek.NoticeFramework.Tools
{
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public class RegulatedRequestSender<TNotice> : RegulatedNoticeSender<TNotice, INoticeReceiver>
        where TNotice : DirectNotice, new()
    {
        #region Constructors
        public RegulatedRequestSender(INoticeReceiver transmitter) : base(transmitter) { }
        public RegulatedRequestSender(INoticeReceiver transmitter, double cooldown) : base(transmitter, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            transmitter.Send(nextMessage);
        }
        #endregion
    }
}

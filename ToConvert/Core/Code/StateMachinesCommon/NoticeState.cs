namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Utils.Tools;
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.Tools;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public abstract class NoticeState<TTrigger, TNotice> : EmptyState<TTrigger>
        where TNotice : BroadcastNotice, new()
    {
        #region Fields
        private TimeOutTicker timeOutTicker;
        private NoticeBroadcaster<TNotice> noticeBroadcaster;
        #endregion

        #region Constructors
        public NoticeState(INoticeReceiver noticeReceiver, int timeOutTicker = -1) : base()
        {
            this.timeOutTicker = timeOutTicker;
            this.noticeBroadcaster = new NoticeBroadcaster<TNotice>(noticeReceiver);
        }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            base.Begin();

            this.timeOutTicker.Begin();
            this.noticeBroadcaster.Broadcast();
        }

        public override void Execute()
        {
            base.Execute();

            if (this.timeOutTicker.CanTrigger())
            {
                TimeOutTriggered();
            }
        }

        protected abstract void TimeOutTriggered();
        #endregion
    }
}

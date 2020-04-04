namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    using Mayfair.Core.Code.StateMachines.FSM.Common;
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;

    internal class NoticeState<TNotice> : NoticeState<LoadingProcessTrigger, TNotice>
        where TNotice : BroadcastNotice, new()
    {
        #region Constructors
        public NoticeState(INoticeReceiver noticeReceiver, int timeOut = -1) : base(noticeReceiver, timeOut) { }
        #endregion

        #region Class Methods
        protected override void TimeOutTriggered()
        {
            Trigger(true);
        }
        #endregion
    }
}

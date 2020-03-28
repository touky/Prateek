namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Messaging.Tools;
    using Mayfair.Core.Code.Utils.Tools;

    public abstract class NoticeState<TTrigger, TNotice> : EmptyState<TTrigger>
        where TNotice : BroadcastMessage, new()
    {
        #region Fields
        private TimeOutTicker timeOutTicker;
        private NoticeBroadcaster<TNotice> noticeBroadcaster;
        #endregion

        #region Constructors
        public NoticeState(IMessageCommunicator communicator, int timeOutTicker = -1) : base()
        {
            this.timeOutTicker = timeOutTicker;
            this.noticeBroadcaster = new NoticeBroadcaster<TNotice>(communicator);
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

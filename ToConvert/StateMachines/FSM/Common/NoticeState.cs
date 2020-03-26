namespace Assets.Prateek.ToConvert.StateMachines.FSM.Common
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.Messaging.Tools;
    using Assets.Prateek.ToConvert.SaveGame.StateMachine;

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
            noticeBroadcaster = new NoticeBroadcaster<TNotice>(communicator);
        }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            base.Begin();

            timeOutTicker.Begin();
            noticeBroadcaster.Broadcast();
        }

        public override void Execute()
        {
            base.Execute();

            if (timeOutTicker.CanTrigger())
            {
                TimeOutTriggered();
            }
        }

        protected abstract void TimeOutTriggered();
        #endregion
    }
}

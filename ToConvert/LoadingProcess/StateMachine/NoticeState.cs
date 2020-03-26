namespace Assets.Prateek.ToConvert.LoadingProcess.StateMachine
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.StateMachines.FSM.Common;

    internal class NoticeState<TNotice> : NoticeState<LoadingProcessTrigger, TNotice>
        where TNotice : BroadcastMessage, new()
    {
        #region Constructors
        public NoticeState(IMessageCommunicator communicator, int timeOut = -1) : base(communicator, timeOut) { }
        #endregion

        #region Class Methods
        protected override void TimeOutTriggered()
        {
            Trigger(true);
        }
        #endregion
    }
}

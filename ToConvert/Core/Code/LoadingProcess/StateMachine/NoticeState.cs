namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.StateMachines.FSM.Common;

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

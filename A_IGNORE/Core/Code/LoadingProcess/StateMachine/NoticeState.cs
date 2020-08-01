namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    using Mayfair.Core.Code.StateMachines.FSM.Common;
    using Commands.Core;
    using Prateek.CommandFramework.TransmitterReceiver;

    internal class NoticeState<TNotice> : NoticeState<LoadingProcessTrigger, TNotice>
        where TNotice : BroadcastCommand, new()
    {
        #region Constructors
        public NoticeState(ICommandReceiver commandReceiver, int timeOut = -1) : base(commandReceiver, timeOut) { }
        #endregion

        #region Class Methods
        protected override void TimeOutTriggered()
        {
            Trigger(true);
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Utils.Tools;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.CommandFramework.Tools;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    public abstract class NoticeState<TTrigger, TNotice> : IdleState<TTrigger>
        where TNotice : BroadcastCommand, new()
    {
        #region Fields
        private TimeOutTicker timeOutTicker;
        private BottledCommandBroadcaster<TNotice> bottledCommandBroadcaster;
        #endregion

        #region Constructors
        public NoticeState(ICommandReceiver commandReceiver, int timeOutTicker = -1) : base()
        {
            this.timeOutTicker = timeOutTicker;
            this.bottledCommandBroadcaster = new BottledCommandBroadcaster<TNotice>(commandReceiver);
        }
        #endregion

        #region Class Methods
        protected override void BeginState()
        {
            base.BeginState();

            this.timeOutTicker.Begin();
            this.bottledCommandBroadcaster.Broadcast();
        }

        protected override void ExecuteState()
        {
            base.ExecuteState();

            if (this.timeOutTicker.CanTrigger())
            {
                TimeOutTriggered();
            }
        }

        protected abstract void TimeOutTriggered();
        #endregion
    }
}

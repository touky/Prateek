namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Utils.Tools;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;
    using Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine.Common;

    public abstract class NoticeState<TTrigger, TNotice> : EmptyState<TTrigger>
        where TNotice : BroadcastCommand, new()
    {
        #region Fields
        private TimeOutTicker timeOutTicker;
        private NoticeBroadcaster<TNotice> noticeBroadcaster;
        #endregion

        #region Constructors
        public NoticeState(ICommandReceiver commandReceiver, int timeOutTicker = -1) : base()
        {
            this.timeOutTicker = timeOutTicker;
            this.noticeBroadcaster = new NoticeBroadcaster<TNotice>(commandReceiver);
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

namespace Mayfair.Core.Code.SaveGame.StateMachine
{
    using Mayfair.Core.Code.Utils.Tools;

    internal abstract class TimeOutState : ServiceState
    {
        #region Fields
        protected TimeOutTicker timeOutTicker;
        #endregion

        #region Constructors
        protected TimeOutState(SaveDaemonCore daemonCore, int timeOutTicker = -1) : base(daemonCore)
        {
            this.timeOutTicker = timeOutTicker;
        }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            this.timeOutTicker.Begin();
        }

        public override void Execute()
        {
            if (this.timeOutTicker.CanTrigger())
            {
                TimeOutTriggered();
            }
        }

        protected abstract void TimeOutTriggered();
        #endregion
    }
}

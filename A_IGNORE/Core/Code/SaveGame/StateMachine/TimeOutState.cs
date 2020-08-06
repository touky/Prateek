namespace Mayfair.Core.Code.SaveGame.StateMachine
{
    using Mayfair.Core.Code.Utils.Tools;

    internal abstract class TimeOutState : ServiceState
    {
        #region Fields
        protected TimeOutTicker timeOutTicker;
        #endregion

        #region Constructors
        protected TimeOutState(SaveDaemon daemonCore, int timeOutTicker = -1) : base(daemonCore)
        {
            this.timeOutTicker = timeOutTicker;
        }
        #endregion

        #region Class Methods
        protected override void BeginState()
        {
            this.timeOutTicker.Begin();
        }

        protected override void ExecuteState()
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

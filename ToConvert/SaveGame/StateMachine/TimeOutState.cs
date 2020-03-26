namespace Assets.Prateek.ToConvert.SaveGame.StateMachine
{
    internal abstract class TimeOutState : ServiceState
    {
        #region Fields
        protected TimeOutTicker timeOutTicker;
        #endregion

        #region Constructors
        protected TimeOutState(SaveService service, int timeOutTicker = -1) : base(service)
        {
            this.timeOutTicker = timeOutTicker;
        }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            timeOutTicker.Begin();
        }

        public override void Execute()
        {
            if (timeOutTicker.CanTrigger())
            {
                TimeOutTriggered();
            }
        }

        protected abstract void TimeOutTriggered();
        #endregion
    }
}

namespace Assets.Prateek.ToConvert.StateMachines.FSM
{
    public abstract class FiniteTransition<TTrigger> : FiniteState<TTrigger>.FiniteTransition
    {
        #region Fields
        private bool hasTriggered = false;
        private bool enabled = true;
        #endregion

        #region Properties
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public override bool HasTriggered
        {
            get { return hasTriggered; }
        }

        public override bool IsUsable
        {
            get { return enabled && TargetState.Enabled; }
        }
        #endregion

        #region Class Methods
        protected void Trigger()
        {
            if (IsUsable)
            {
                hasTriggered = true;
            }
        }

        public override FiniteState<TTrigger> TryConsumingTrigger()
        {
            hasTriggered = false;
            return base.TryConsumingTrigger();
        }
        #endregion
    }
}

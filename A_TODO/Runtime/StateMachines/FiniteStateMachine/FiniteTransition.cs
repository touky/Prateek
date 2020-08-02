namespace Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine
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
            get { return this.enabled; }
            set { this.enabled = value; }
        }

        public override bool HasTriggered
        {
            get { return this.hasTriggered; }
        }

        public override bool IsUsable
        {
            get { return this.enabled && this.TargetState.Enabled; }
        }
        #endregion

        #region Class Methods
        protected void Trigger()
        {
            if (IsUsable)
            {
                this.hasTriggered = true;
            }
        }

        public override FiniteState<TTrigger> TryConsumingTrigger()
        {
            this.hasTriggered = false;
            return base.TryConsumingTrigger();
        }
        #endregion
    }
}

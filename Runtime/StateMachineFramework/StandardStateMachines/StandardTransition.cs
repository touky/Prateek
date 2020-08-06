namespace Prateek.Runtime.StateMachineFramework.StandardStateMachines
{
    public abstract class StandardTransition<TTrigger>
    {
        #region Fields
        private bool hasTriggered = false;
        private bool enabled = true;
        private StandardState<TTrigger> targetState;
        #endregion

        #region Properties
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public virtual bool HasTriggered
        {
            get { return hasTriggered; }
        }

        public virtual bool IsUsable
        {
            get { return enabled && TargetState.Enabled; }
        }

        public StandardState<TTrigger> TargetState
        {
            get { return targetState; }
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

        protected void ConsumeTrigger()
        {
            hasTriggered = false;
        }

        internal virtual StandardState<TTrigger> TryConsumingTrigger()
        {
            ConsumeTrigger();

            return targetState;
        }

        public StandardTransition<TTrigger> From(StandardState<TTrigger> from)
        {
            from.AddTransition(this);
            return this;
        }

        public StandardTransition<TTrigger> To(StandardState<TTrigger> to)
        {
            targetState = to;
            return this;
        }

        internal void Trigger(TTrigger trigger)
        {
            ValidateTrigger(trigger);
        }

        protected abstract void ValidateTrigger(TTrigger trigger);
        #endregion
    }
}

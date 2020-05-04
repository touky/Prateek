namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    public class ContainerState<TTrigger> : FiniteState<TTrigger>
    {
        #region Fields
        private FiniteStateMachine<TTrigger> stateMachine;
        #endregion

        #region Properties
        public override bool Enabled
        {
            protected set
            {
                if (value != base.Enabled)
                {
                    End();
                }

                base.Enabled = value;
            }
        }
        #endregion

        #region Constructors
        public ContainerState(FiniteState<TTrigger> startState)
        {
            this.stateMachine = new FiniteStateMachine<TTrigger>(startState);
        }
        #endregion

        #region Class Methods
        public void Add(FiniteState<TTrigger> state)
        {
            this.stateMachine.Add(state);
        }

        public void Remove(FiniteState<TTrigger> state)
        {
            this.stateMachine.Remove(state);
        }

        public override void Trigger(TTrigger trigger)
        {
            base.Trigger(trigger);

            this.stateMachine.Trigger(trigger);
        }

        protected override void Begin()
        {
            this.stateMachine.Restart();
        }

        protected override void End()
        {
            this.stateMachine.Restart();
        }

        public override void Execute()
        {
            this.stateMachine.Advance();
        }
        #endregion
    }
}

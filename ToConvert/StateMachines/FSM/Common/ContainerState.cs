namespace Assets.Prateek.ToConvert.StateMachines.FSM.Common
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
            stateMachine = new FiniteStateMachine<TTrigger>(startState);
        }
        #endregion

        #region Class Methods
        public void Add(FiniteState<TTrigger> state)
        {
            stateMachine.Add(state);
        }

        public void Remove(FiniteState<TTrigger> state)
        {
            stateMachine.Remove(state);
        }

        public override void Trigger(TTrigger trigger)
        {
            base.Trigger(trigger);

            stateMachine.Trigger(trigger);
        }

        protected override void Begin()
        {
            stateMachine.Restart();
        }

        protected override void End()
        {
            stateMachine.Restart();
        }

        public override void Execute()
        {
            stateMachine.Advance();
        }
        #endregion
    }
}

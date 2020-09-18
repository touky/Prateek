namespace Prateek.Runtime.StateMachineFramework.StandardStateMachines
{
    public class InnerMachineState<TTrigger> : StandardState<TTrigger>
    {
        #region Fields
        private StandardStateMachine<TTrigger> stateMachine;
        #endregion

        #region Properties
        public override bool Enabled
        {
            protected set
            {
                if (value != base.Enabled)
                {
                    EndState();
                }

                base.Enabled = value;
            }
        }
        #endregion

        #region Constructors
        public InnerMachineState(StandardState<TTrigger> startState)
        {
            stateMachine = new StandardStateMachine<TTrigger>(startState);
        }
        #endregion

        #region Class Methods
        public void Add(StandardState<TTrigger> state)
        {
            stateMachine.Add(state);
        }

        public void Remove(StandardState<TTrigger> state)
        {
            stateMachine.Remove(state);
        }

        public override void Trigger(TTrigger trigger)
        {
            base.Trigger(trigger);

            stateMachine.Trigger(trigger);
        }

        protected override void BeginState()
        {
            stateMachine.Reboot();
        }

        protected override void ExecuteState()
        {
            stateMachine.Step();
        }

        protected override void EndState()
        {
            stateMachine.Reboot();
        }
        #endregion
    }
}

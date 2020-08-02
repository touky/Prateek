namespace Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine.BoolTrigger
{
    public class BoolTriggerTransition : FiniteTransition<bool>
    {
        #region Constructors
        public BoolTriggerTransition() { }
        #endregion

        #region Class Methods
        public override void TryTriggering(bool trigger)
        {
            if (trigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

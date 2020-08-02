namespace Mayfair.Core.Code.SaveGame.StateMachine
{
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine;

    public class SaveTransition : FiniteTransition<SaveState>
    {
        #region Fields
        private SaveState triggerState;
        #endregion

        #region Constructors
        public SaveTransition(SaveState triggerState = SaveState.NextState)
        {
            this.triggerState = triggerState;
        }
        #endregion

        #region Class Methods
        public override void TryTriggering(SaveState trigger)
        {
            if (this.triggerState == trigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

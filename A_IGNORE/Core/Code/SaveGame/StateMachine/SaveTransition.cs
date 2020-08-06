namespace Mayfair.Core.Code.SaveGame.StateMachine
{
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    public class SaveTransition : StandardTransition<SaveState>
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
        protected override void ValidateTrigger(SaveState trigger)
        {
            if (this.triggerState == trigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

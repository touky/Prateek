namespace Assets.Prateek.ToConvert.SaveGame.StateMachine
{
    using Assets.Prateek.ToConvert.SaveGame.Enums;
    using Assets.Prateek.ToConvert.StateMachines.FSM;

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
            if (triggerState == trigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

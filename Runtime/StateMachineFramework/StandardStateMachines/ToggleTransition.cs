namespace Prateek.Runtime.StateMachineFramework.StandardStateMachines
{
    public class ToggleTransition : StandardTransition<bool>
    {
        #region Constructors
        #endregion

        #region Class Methods
        protected override void ValidateTrigger(bool trigger)
        {
            if (trigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

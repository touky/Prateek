namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    internal class LoadingBoolTransition : TriggerTransition
    {
        #region Class Methods
        protected override void ValidateTrigger(LoadingProcessTrigger trigger)
        {
            if (trigger.doTrigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

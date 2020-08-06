namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    internal class GameLoadingRestartTransition : TriggerTransition
    {
        #region Class Methods
        protected override void ValidateTrigger(LoadingProcessTrigger trigger)
        {
            if (trigger.notice != null)
            {
                Trigger();
            }
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    internal class GameLoadingRestartTransition : TriggerTransition
    {
        #region Class Methods
        public override void TryTriggering(LoadingProcessTrigger trigger)
        {
            if (trigger.notice != null)
            {
                Trigger();
            }
        }
        #endregion
    }
}

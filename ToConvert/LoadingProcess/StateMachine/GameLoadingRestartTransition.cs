namespace Assets.Prateek.ToConvert.LoadingProcess.StateMachine
{
    internal class GameLoadingRestartTransition : TriggerTransition
    {
        #region Class Methods
        public override void TryTriggering(LoadingProcessTrigger trigger)
        {
            if (trigger.message != null)
            {
                Trigger();
            }
        }
        #endregion
    }
}

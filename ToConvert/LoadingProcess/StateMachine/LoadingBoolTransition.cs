namespace Assets.Prateek.ToConvert.LoadingProcess.StateMachine
{
    internal class LoadingBoolTransition : TriggerTransition
    {
        #region Class Methods
        public override void TryTriggering(LoadingProcessTrigger trigger)
        {
            if (trigger.doTrigger)
            {
                Trigger();
            }
        }
        #endregion
    }
}

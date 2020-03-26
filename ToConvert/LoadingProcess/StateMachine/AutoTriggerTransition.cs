namespace Assets.Prateek.ToConvert.LoadingProcess.StateMachine
{
    internal class AutoTriggerTransition : TriggerTransition
    {
        #region Fields
        private bool triggerNextTime = false;
        #endregion

        #region Class Methods
        public override void TryTriggering(LoadingProcessTrigger trigger)
        {
            if (triggerNextTime)
            {
                Trigger();
            }

            triggerNextTime = !triggerNextTime;
        }
        #endregion
    }
}

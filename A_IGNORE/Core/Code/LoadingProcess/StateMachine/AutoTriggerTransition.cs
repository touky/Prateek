namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    internal class AutoTriggerTransition : TriggerTransition
    {
        #region Fields
        private bool triggerNextTime = false;
        #endregion

        #region Class Methods
        protected override void ValidateTrigger(LoadingProcessTrigger trigger)
        {
            if (this.triggerNextTime)
            {
                Trigger();
            }

            this.triggerNextTime = !this.triggerNextTime;
        }
        #endregion
    }
}

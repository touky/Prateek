namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    internal class AutoTriggerTransition : TriggerTransition
    {
        #region Fields
        private bool triggerNextTime = false;
        #endregion

        #region Class Methods
        public override void TryTriggering(LoadingProcessTrigger trigger)
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

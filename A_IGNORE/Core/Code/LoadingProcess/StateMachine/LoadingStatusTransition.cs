namespace Mayfair.Core.Code.LoadingProcess.StateMachine
{
    using Mayfair.Core.Code.LoadingProcess.Enums;

    internal class LoadingStatusTransition : TriggerTransition
    {
        #region Fields
        private LoadingTrackingStatus trackerStatus;
        #endregion

        #region Constructors
        public LoadingStatusTransition(LoadingTrackingStatus trackerStatus)
        {
            this.trackerStatus = trackerStatus;
        }
        #endregion

        #region Class Methods
        protected override void ValidateTrigger(LoadingProcessTrigger trigger)
        {
            if (trigger.trackerStatus >= this.trackerStatus)
            {
                Trigger();
            }
        }
        #endregion
    }
}

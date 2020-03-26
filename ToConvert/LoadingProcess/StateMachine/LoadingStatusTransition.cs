namespace Assets.Prateek.ToConvert.LoadingProcess.StateMachine
{
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;

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
        public override void TryTriggering(LoadingProcessTrigger trigger)
        {
            if (trigger.trackerStatus >= trackerStatus)
            {
                Trigger();
            }
        }
        #endregion
    }
}

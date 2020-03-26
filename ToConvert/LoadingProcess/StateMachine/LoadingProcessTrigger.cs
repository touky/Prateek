namespace Assets.Prateek.ToConvert.LoadingProcess.StateMachine
{
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;

    /// <summary>
    ///     WARNING: THIS IS A TEMPORARY VERSION OF THIS (29/11/19)
    ///     DO NOT use that pattern as a standard of how to do FSM trigger (Talk with jonh || benjaminh about it)
    /// </summary>
    internal struct LoadingProcessTrigger
    {
        public bool doTrigger;
        public LoadingTrackingStatus trackerStatus;
        public GameLoadingNeedRestart message;

        public static implicit operator LoadingProcessTrigger(bool doTrigger)
        {
            return new LoadingProcessTrigger {doTrigger = doTrigger};
        }

        public static implicit operator LoadingProcessTrigger(LoadingTrackingStatus trackerStatus)
        {
            return new LoadingProcessTrigger {trackerStatus = trackerStatus};
        }

        public static implicit operator LoadingProcessTrigger(GameLoadingNeedRestart message)
        {
            return new LoadingProcessTrigger {message = message};
        }
    }
}

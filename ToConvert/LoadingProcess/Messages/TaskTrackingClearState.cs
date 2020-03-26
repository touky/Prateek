namespace Assets.Prateek.ToConvert.LoadingProcess.Messages
{
    using Assets.Prateek.ToConvert.LoadingProcess.StateMachine;
    using Assets.Prateek.ToConvert.StateMachines.FSM.Common;

    internal class TaskTrackingClearState : ServiceState<LoadingProcessTrigger, LoadingProcessService>
    {
        #region Constructors
        public TaskTrackingClearState(LoadingProcessService service) : base(service) { }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            this.service.ClearTaskTracking();
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Mayfair.Core.Code.LoadingProcess.StateMachine;
    using Mayfair.Core.Code.StateMachines.FSM.Common;

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

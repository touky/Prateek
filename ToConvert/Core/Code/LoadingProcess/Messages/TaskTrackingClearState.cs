namespace Mayfair.Core.Code.LoadingProcess.Messages
{
    using Mayfair.Core.Code.LoadingProcess.StateMachine;
    using Mayfair.Core.Code.StateMachines.FSM.Common;

    internal class TaskTrackingClearState : ServiceState<LoadingProcessTrigger, LoadingProcessDaemonCore>
    {
        #region Constructors
        public TaskTrackingClearState(LoadingProcessDaemonCore daemonCore) : base(daemonCore) { }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            this.daemonCore.ClearTaskTracking();
        }
        #endregion
    }
}

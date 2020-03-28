namespace Mayfair.Core.Code.LoadingProcess
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM.Common;
    using Prateek.DaemonCore.Code.Branches;

    public abstract class LoadingProcessDaemonBranch : DaemonBranchBehaviour<LoadingProcessDaemonCore, LoadingProcessDaemonBranch>
    {
        #region Fields
        private LoadingProcessStatus processStatus;
        private bool initHasEnded = false;
        #endregion

        #region Properties
        protected override bool IsAliveInternal
        {
            get { return !this.initHasEnded; }
        }

        protected bool LoadingHasEnded
        {
            set { this.initHasEnded = value; }
        }
        #endregion

        #region Class Methods
        public abstract void UpdateProcess(List<LoadingTaskTracker> trackers);

        public void Init(LoadingProcessDaemonCore daemonCore)
        {
            if (this.processStatus != LoadingProcessStatus.None)
            {
                return;
            }

            InternalInit(daemonCore);

            this.processStatus = LoadingProcessStatus.Idle;
        }

        protected abstract void InternalInit(LoadingProcessDaemonCore daemonCore);
        public abstract void GameLoadingRestart(GameLoadingNeedRestart message);
        #endregion

        #region Nested type: LoadingEndState
        //Protected to be accessible by the children since LoadingHasEnded is protected
        protected class LoadingStatusState<TTrigger> : EmptyState<TTrigger>
        {
            #region Fields
            protected LoadingProcessDaemonBranch branch;
            private bool shouldEnd;
            #endregion

            #region Constructors
            public LoadingStatusState(LoadingProcessDaemonBranch branch, bool shouldEnd)
            {
                this.branch = branch;
                this.shouldEnd = shouldEnd;
            }
            #endregion

            #region Class Methods
            public override void Execute()
            {
                base.Execute();

                this.branch.LoadingHasEnded = this.shouldEnd;
            }
            #endregion
        }
        #endregion
    }
}

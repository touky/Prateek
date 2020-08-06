namespace Mayfair.Core.Code.LoadingProcess
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM.Common;
    using Prateek.Runtime.DaemonFramework.Servants;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    public abstract class LoadingProcessServant : ServantTickable<LoadingProcessDaemon, LoadingProcessServant>
    {
        #region Fields
        private LoadingProcessStatus processStatus;
        private bool initHasEnded = false;
        #endregion

        #region Properties
        public override bool IsAlive
        {
            get { return !this.initHasEnded && base.IsAlive; }
        }

        protected bool LoadingHasEnded
        {
            set { this.initHasEnded = value; }
        }
        #endregion

        #region Class Methods
        public abstract void UpdateProcess(List<LoadingTaskTracker> trackers);

        public void Init(LoadingProcessDaemon daemonCore)
        {
            if (this.processStatus != LoadingProcessStatus.None)
            {
                return;
            }

            InternalInit(daemonCore);

            this.processStatus = LoadingProcessStatus.Idle;
        }

        protected abstract void InternalInit(LoadingProcessDaemon daemonCore);
        public abstract void GameLoadingRestart(GameLoadingNeedRestart notice);
        #endregion

        #region Nested type: LoadingEndState
        //Protected to be accessible by the children since LoadingHasEnded is protected
        protected class LoadingStatusState<TTrigger> : IdleState<TTrigger>
        {
            #region Fields
            protected LoadingProcessServant servant;
            private bool shouldEnd;
            #endregion

            #region Constructors
            public LoadingStatusState(LoadingProcessServant servant, bool shouldEnd)
            {
                this.servant = servant;
                this.shouldEnd = shouldEnd;
            }
            #endregion

            #region Class Methods
            protected override void ExecuteState()
            {
                base.ExecuteState();

                this.servant.LoadingHasEnded = this.shouldEnd;
            }
            #endregion
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.LoadingProcess
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM.Common;

    public abstract class LoadingProcessServiceProvider : ServiceProviderBehaviour
    {
        #region Fields
        private LoadingProcessStatus processStatus;
        private bool initHasEnded = false;
        #endregion

        #region Properties
        public override bool IsProviderValid
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

        public void Init(LoadingProcessService service)
        {
            if (this.processStatus != LoadingProcessStatus.None)
            {
                return;
            }

            InternalInit(service);

            this.processStatus = LoadingProcessStatus.Idle;
        }

        protected abstract void InternalInit(LoadingProcessService service);
        public abstract void GameLoadingRestart(GameLoadingNeedRestart message);
        #endregion

        #region Nested type: LoadingEndState
        //Protected to be accessible by the children since LoadingHasEnded is protected
        protected class LoadingStatusState<TTrigger> : EmptyState<TTrigger>
        {
            #region Fields
            protected LoadingProcessServiceProvider provider;
            private bool shouldEnd;
            #endregion

            #region Constructors
            public LoadingStatusState(LoadingProcessServiceProvider provider, bool shouldEnd)
            {
                this.provider = provider;
                this.shouldEnd = shouldEnd;
            }
            #endregion

            #region Class Methods
            public override void Execute()
            {
                base.Execute();

                this.provider.LoadingHasEnded = this.shouldEnd;
            }
            #endregion
        }
        #endregion
    }
}

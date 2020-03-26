namespace Assets.Prateek.ToConvert.LoadingProcess
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;
    using Assets.Prateek.ToConvert.Service;
    using Assets.Prateek.ToConvert.StateMachines.FSM.Common;

    public abstract class LoadingProcessServiceProvider : ServiceProviderBehaviour
    {
        #region Fields
        private LoadingProcessStatus processStatus;
        private bool initHasEnded = false;
        #endregion

        #region Properties
        public override bool IsProviderValid
        {
            get { return !initHasEnded; }
        }

        protected bool LoadingHasEnded
        {
            set { initHasEnded = value; }
        }
        #endregion

        #region Class Methods
        public abstract void UpdateProcess(List<LoadingTaskTracker> trackers);

        public void Init(LoadingProcessService service)
        {
            if (processStatus != LoadingProcessStatus.None)
            {
                return;
            }

            InternalInit(service);

            processStatus = LoadingProcessStatus.Idle;
        }

        protected abstract void InternalInit(LoadingProcessService service);
        public abstract void GameLoadingRestart(GameLoadingNeedRestart message);
        #endregion

        #region Nested type: LoadingStatusState
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

                provider.LoadingHasEnded = shouldEnd;
            }
            #endregion
        }
        #endregion
    }
}

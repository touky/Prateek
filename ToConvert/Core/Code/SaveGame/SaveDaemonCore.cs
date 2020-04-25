namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.SaveGame.Messages;
    using Mayfair.Core.Code.SaveGame.StateMachine;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.Tools;
    using Prateek.TickableFramework.Code.Enums;

    public class SaveDaemonCore : NoticeReceiverDaemonCore<SaveDaemonCore, SaveDaemonBranch>
    {
        #region Fields
        private FiniteStateMachine<SaveState> stateMachine;

        private List<LoadDataRequest> loadingRequests = new List<LoadDataRequest>();
        #endregion

        
        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }

        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            this.stateMachine.Advance();
        }

        #region Class Methods
        protected override void OnAwake()
        {
            SaveIdleState idle = new SaveIdleState();

            //Full loading FSM
            {
                LoadPendingRequestState loadPendingRequests = new LoadPendingRequestState(this, Consts.WAIT_5_FRAMES);
                SendLoadedResponseState sendLoadedResponses = new SendLoadedResponseState(this);

                new SaveTransition(SaveState.LoadLoop).From(idle).To(loadPendingRequests);
                new SaveTransition().From(loadPendingRequests).To(sendLoadedResponses);
                new SaveTransition().From(sendLoadedResponses).To(idle);
            }

            this.stateMachine = new FiniteStateMachine<SaveState>(idle);
            this.stateMachine.Advance();
        }

        protected bool TryLoadingPendingRequest()
        {
            bool loadedAll = true;
            IEnumerable<SaveDaemonBranch> providers = GetValidBranches();
            foreach (SaveDaemonBranch branch in providers)
            {
                for (int r = 0; r < this.loadingRequests.Count; r++)
                {
                    LoadDataRequest request = this.loadingRequests[r];
                    loadedAll = branch.TryLoad(request.Identifications) && loadedAll;
                }
            }

            if (!loadedAll)
            {
                foreach (LoadDataRequest request in this.loadingRequests)
                {
                    foreach (SaveDataIdentification identification in request.Identifications)
                    {
                        //If the load hasn't even failed, create an empty save
                        if (identification.Status == SaveDataStatusType.None)
                        {
                            identification.CreateEmpty();
                        }
                    }
                }
            }

            return loadedAll;
        }

        protected void SendLoadingResponse()
        {
            for (int r = 0; r < this.loadingRequests.Count; r++)
            {
                LoadDataRequest request = this.loadingRequests[r];
                ResponseNotice response = request.GetResponse();
                response.Init(request);
                NoticeReceiver.Send(response);
            }

            this.loadingRequests.Clear();
        }

        public override void NoticeReceived() { }

        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<LoadDataRequest>(OnLoadingRequest);
        }

        private void OnLoadingRequest(LoadDataRequest request)
        {
            //TODO: this can be lost
            this.loadingRequests.Add(request);
            this.stateMachine.Trigger(SaveState.LoadLoop);
        }
        #endregion

        #region Nested type: LoadDataState
        private class LoadPendingRequestState : TimeOutState
        {
            #region Constructors
            public LoadPendingRequestState(SaveDaemonCore daemonCore, int timeOut = -1) : base(daemonCore, timeOut) { }
            #endregion

            #region Class Methods
            public override void Execute()
            {
                base.Execute();

                if (!this.daemonCore.TryLoadingPendingRequest())
                {
                    this.timeOutTicker.Begin();
                }
            }

            protected override void TimeOutTriggered()
            {
                Trigger(SaveState.NextState);
            }
            #endregion
        }
        #endregion

        #region Nested type: SendDataResponseState
        private class SendLoadedResponseState : ServiceState
        {
            #region Constructors
            public SendLoadedResponseState(SaveDaemonCore daemonCore) : base(daemonCore) { }
            #endregion

            #region Class Methods
            public override void Execute()
            {
                this.daemonCore.SendLoadingResponse();

                Trigger(SaveState.NextState);
            }
            #endregion
        }
        #endregion
    }
}

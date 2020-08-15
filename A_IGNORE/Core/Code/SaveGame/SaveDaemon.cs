namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.SaveGame.Messages;
    using Mayfair.Core.Code.SaveGame.StateMachine;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    using Prateek.Runtime.TickableFramework.Interfaces;

    public sealed class SaveDaemon
        : DaemonOverseer<SaveDaemon, SaveServant>
        , ICommandReceiverOwner
        , IPreUpdateTickable
    {
        #region Fields
        private StandardStateMachine<SaveState> stateMachine;

        private List<LoadDataRequest> loadingRequests = new List<LoadDataRequest>();
        #endregion

        public void PreUpdate()
        {
            this.stateMachine.Step();
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

            this.stateMachine = new StandardStateMachine<SaveState>(idle);
            this.stateMachine.Step();
        }

        protected bool TryLoadingPendingRequest()
        {
            bool loadedAll = true;
            foreach (var servant in AllAliveServants)
            {
                for (int r = 0; r < this.loadingRequests.Count; r++)
                {
                    LoadDataRequest request = this.loadingRequests[r];
                    loadedAll = servant.TryLoad(request.Identifications) && loadedAll;
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
                LoadDataResponse response = request.GetResponse<LoadDataResponse>();
                response.Init(request);
                this.Get<ICommandReceiver>().Send(response);
            }

            this.loadingRequests.Clear();
        }

        public void DefineReceptionActions(ICommandReceiver receiver)
        {
            receiver.SetActionFor<LoadDataRequest>(OnLoadingRequest);
        }

        private void OnLoadingRequest(LoadDataRequest request)
        {
            //TODO: this can be lost
            this.loadingRequests.Add(request);
            this.stateMachine.Trigger(SaveState.LoadLoop);
        }

        public int Priority(IPriority<IApplicationFeedbackTickable> type)
        {
            throw new System.NotImplementedException();
        }

        public int Priority(IPriority<IPreUpdateTickable> type)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Nested type: LoadDataState
        private class LoadPendingRequestState : TimeOutState
        {
            #region Constructors
            public LoadPendingRequestState(SaveDaemon daemonCore, int timeOut = -1) : base(daemonCore, timeOut) { }
            #endregion

            #region Class Methods
            protected override void ExecuteState()
            {
                base.ExecuteState();

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
            public SendLoadedResponseState(SaveDaemon daemonCore) : base(daemonCore) { }
            #endregion

            #region Class Methods
            protected override void ExecuteState()
            {
                this.daemonCore.SendLoadingResponse();

                Trigger(SaveState.NextState);
            }
            #endregion
        }
        #endregion
    }
}

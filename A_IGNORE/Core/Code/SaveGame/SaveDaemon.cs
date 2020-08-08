namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.SaveGame.Enums;
    using Mayfair.Core.Code.SaveGame.Messages;
    using Mayfair.Core.Code.SaveGame.StateMachine;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    using Prateek.Runtime.TickableFramework.Interfaces;

    public class SaveDaemon
        : ReceiverDaemonOverseer<SaveDaemon, SaveServant>
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
                CommandReceiver.Send(response);
            }

            this.loadingRequests.Clear();
        }

        public override void DefineCommandReceiverActions()
        {
            CommandReceiver.SetActionFor<LoadDataRequest>(OnLoadingRequest);
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

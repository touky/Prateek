namespace Assets.Prateek.ToConvert.SaveGame
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.SaveGame.Enums;
    using Assets.Prateek.ToConvert.SaveGame.Messages;
    using Assets.Prateek.ToConvert.SaveGame.StateMachine;
    using Assets.Prateek.ToConvert.Service;
    using Assets.Prateek.ToConvert.StateMachines.FSM;

    public class SaveService : ServiceCommunicatorBehaviour<SaveService, SaveServiceProvider>
    {
        #region Fields
        private FiniteStateMachine<SaveState> stateMachine;

        private List<LoadDataRequest> loadingRequests = new List<LoadDataRequest>();
        #endregion

        #region Unity Methods
        protected override void Update()
        {
            base.Update();

            stateMachine.Advance();
        }
        #endregion

        #region Service
        protected override void OnAwake()
        {
            var idle = new SaveIdleState();

            //Full loading FSM
            {
                var loadPendingRequests = new LoadPendingRequestState(this, Consts.WAIT_5_FRAMES);
                var sendLoadedResponses = new SendLoadedResponseState(this);

                new SaveTransition(SaveState.LoadLoop).From(idle).To(loadPendingRequests);
                new SaveTransition().From(loadPendingRequests).To(sendLoadedResponses);
                new SaveTransition().From(sendLoadedResponses).To(idle);
            }

            stateMachine = new FiniteStateMachine<SaveState>(idle);
            stateMachine.Advance();
        }
        #endregion

        #region Messaging
        public override void MessageReceived() { }

        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<LoadDataRequest>(OnLoadingRequest);
        }
        #endregion

        #region Class Methods
        protected bool TryLoadingPendingRequest()
        {
            var loadedAll = true;
            var providers = GetAllValidProviders();
            foreach (var provider in providers)
            {
                for (var r = 0; r < loadingRequests.Count; r++)
                {
                    var request = loadingRequests[r];
                    loadedAll = provider.TryLoad(request.Identifications) && loadedAll;
                }
            }

            if (!loadedAll)
            {
                foreach (var request in loadingRequests)
                {
                    foreach (var identification in request.Identifications)
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
            for (var r = 0; r < loadingRequests.Count; r++)
            {
                var             request  = loadingRequests[r];
                ResponseMessage response = request.GetResponse();
                response.Init(request);
                Communicator.Send(response);
            }

            loadingRequests.Clear();
        }

        private void OnLoadingRequest(LoadDataRequest request)
        {
            //TODO: this can be lost
            loadingRequests.Add(request);
            stateMachine.Trigger(SaveState.LoadLoop);
        }
        #endregion

        #region Nested type: LoadPendingRequestState
        private class LoadPendingRequestState : TimeOutState
        {
            #region Constructors
            public LoadPendingRequestState(SaveService service, int timeOut = -1) : base(service, timeOut) { }
            #endregion

            #region Class Methods
            public override void Execute()
            {
                base.Execute();

                if (!this.service.TryLoadingPendingRequest())
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

        #region Nested type: SendLoadedResponseState
        private class SendLoadedResponseState : ServiceState
        {
            #region Constructors
            public SendLoadedResponseState(SaveService service) : base(service) { }
            #endregion

            #region Class Methods
            public override void Execute()
            {
                this.service.SendLoadingResponse();

                Trigger(SaveState.NextState);
            }
            #endregion
        }
        #endregion
    }
}

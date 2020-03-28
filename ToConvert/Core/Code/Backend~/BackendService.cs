namespace Mayfair.Core.Code.Backend
{
    using System;
    using System.Collections.Generic;
    using LoadingProcess;
    using LoadingProcess.Enums;
    using LoadingProcess.Messages;
    using Messages;
    using Messaging.Messages;
    using Service;
    using TimeService.Messages;
    using TimeService.ServiceProviders;
    using UnityEngine.Assertions;
    using Utils.Debug;

    public class BackendService : ServiceCommunicatorBehaviour<BackendService, BaseBackendServiceProvider>
    {
        private BaseBackendServiceProvider activeProvider;
        private TaskLoadingMessage backendLoadingMessage;

        public override void MessageReceived() { }

        protected override void OnAwake()
        {
            backendLoadingMessage = Message.Create<TaskLoadingMessage>();
        }

        protected override void OnRegister(BaseBackendServiceProvider provider)
        {
            base.OnRegister(provider);

            Assert.IsNull(activeProvider, "A BackendServiceProvider has already been registered, there should only be one.");

            activeProvider = provider;
        }

        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoadingNotice);
        }

        private void OnGameLoadingNotice(GameLoadingPrerequisiteNotice message)
        {
            SendStatusMessage(LoadingTrackingStatus.StartedLoading);

            activeProvider.Login(OnLoginSuccess, OnLoginFailed);
        }

        private void SendStatusMessage(LoadingTrackingStatus status)
        {
            backendLoadingMessage.trackerState = new LoadingTaskTracker(GetType(), status);
            Communicator.Send(backendLoadingMessage);
        }

        private void OnLoginSuccess(string playerId, bool isNewPlayer)
        {
            DebugTools.Log($"Player logged in with ID {playerId}. Is new player: {isNewPlayer}");

            PlayerLoggedIn message = Message.Create<PlayerLoggedIn>();
            message.Init(playerId, isNewPlayer);
            Communicator.Broadcast(message);

            activeProvider.CreateServerTimeProvider();

            SendStatusMessage(LoadingTrackingStatus.Finished);
        }

        private void OnLoginFailed()
        {
            throw new Exception("Failed to log in");
        }
    }
}
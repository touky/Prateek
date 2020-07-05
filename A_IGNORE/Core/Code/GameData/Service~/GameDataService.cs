namespace Mayfair.Core.Code.GameData
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.GameData.Messages;
    using Mayfair.Core.Code.LoadingProcess;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.SaveGame;
    using Mayfair.Core.Code.SaveGame.Messages;
    using Mayfair.Core.Code.Service;

    public class GameDataService : ServiceCommunicatorBehaviour<GameDataService, GameDataServiceProvider>, IDebugMenuNotebookOwner
    {
        #region Fields
        private Dictionary<Type, TaskLoadingMessage> taskMessages = new Dictionary<Type, TaskLoadingMessage>();
        #endregion

        #region Unity Methods
        private void Start()
        {
            SetupDebugContent();
        }
        #endregion

        #region Service
        protected override void OnAwake() { }

        protected override void OnRegister(GameDataServiceProvider provider)
        {
            base.OnRegister(provider);

            provider.Init(Communicator);
        }
        #endregion

        #region Messaging
        public override void MessageReceived() { }

        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoadingNotice);
            Communicator.AddCallback<GameLoadingGameplayNotice>(OnGameLoadingNotice);
            Communicator.AddCallback<GameLoadingFinalizeNotice>(OnGameLoadingNotice);
            Communicator.AddCallback<GameLoadingRestartNotice>(OnGameLoadingRestart);

            Communicator.AddCallback<LoadDataResponse>(OnLoadData);
            Communicator.AddCallback<GameDataLoaded>(OnGameDataLoaded);
            Communicator.AddCallback<SessionDebugAvailable>(OnSessionDebugAvailable);
        }
        #endregion

        #region Class Methods
        private void SendStatusMessage(GameDataServiceProvider provider, LoadingTrackingStatus status)
        {
            Type type = provider.GetType();
            TaskLoadingMessage message = null;
            if (!taskMessages.ContainsKey(type))
            {
                taskMessages.Add(type, Message.Create<TaskLoadingMessage>());
            }

            taskMessages.TryGetValue(type, out message);

            message.trackerState = new LoadingTaskTracker(type, status);

            Communicator.Send(message);
        }

        private void OnGameLoadingNotice<T>(T message) where T : GameLoadingNotice
        {
            LoadDataRequest request = null;
            IEnumerable<GameDataServiceProvider> providers = GetAllValidProviders();
            foreach (GameDataServiceProvider provider in providers)
            {
                if (!provider.HasRequestForNotice(message))
                {
                    continue;
                }

                if (request == null)
                {
                    request = Message.Create<LoadDataRequest>();
                }

                provider.FillSaveRequest(message, request);

                SendStatusMessage(provider, LoadingTrackingStatus.StartedLoading);
            }

            if (request != null)
            {
                Communicator.Send(request);
            }
        }

        private void OnGameLoadingRestart(GameLoadingRestartNotice message)
        {
            Communicator.Broadcast(Message.Create<GameSessionClose>());
        }

        private void OnLoadData(LoadDataResponse message)
        {
            IEnumerable<GameDataServiceProvider> providers = GetAllValidProviders();
            foreach (GameDataServiceProvider provider in providers)
            {
                foreach (SaveDataIdentification identification in message.Request.Identifications)
                {
                    if (provider.ReceiveIdentificationData(identification))
                    {
                        SendStatusMessage(provider, LoadingTrackingStatus.Finished);
                    }
                }
            }
        }

        private void OnGameDataLoaded(GameDataLoaded message)
        {
            IEnumerable<GameDataServiceProvider> providers = GetAllValidProviders();
            foreach (GameDataServiceProvider provider in providers)
            {
                provider.OnGameDataLoaded(message);
            }
        }

        private void OnSessionDebugAvailable(SessionDebugAvailable message)
        {
            IEnumerable<GameDataServiceProvider> providers = GetAllValidProviders();
            foreach (GameDataServiceProvider provider in providers)
            {
                provider.OnSessionDebugAvailable(message);
            }
        }

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        private void SetupDebugContent()
        {
            DebugMenuNotebook debugNotebook = new DebugMenuNotebook("GAMD", "Game Datas");

            EmptyMenuPage main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new GameDataPage(this, "Data debug"));
            debugNotebook.Register();
        }
        #endregion
        #endregion
    }
}

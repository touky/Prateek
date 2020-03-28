namespace Mayfair.Core.Code.LoadingProcess
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils;

    public sealed class LoadingProcessService : ServiceCommunicatorBehaviour<LoadingProcessService, LoadingProcessServiceProvider>, IDebugMenuNotebookOwner
    {
        #region Fields
        private LoadingProcessStatus loadingStatus = LoadingProcessStatus.Idle;

        //This is intended to be a list to keep an ordered trace of the callers
        private List<LoadingTaskTracker> trackers = new List<LoadingTaskTracker>();
        #endregion

        #region Unity Methods
        private void Start()
        {
            SetupDebugContent();
        }

        protected override void Update()
        {
            base.Update();

            if (!TryUpdatingProvider())
            {
                ChangeStatus(LoadingProcessStatus.Idle);
            }
        }
        #endregion

        #region Service
        #region Init
        protected override void OnAwake() { }
        #endregion
        #endregion

        #region Messaging
        public override void MessageReceived() { }

        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<TaskLoadingMessage>(OnLoadTaskMessage);
            Communicator.AddCallback<GameLoadingNeedRestart>(OnGameLoadingNeedRestart);

            Communicator.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoading);
            Communicator.AddCallback<GameLoadingGameplayNotice>(OnGameLoading);
            Communicator.AddCallback<GameLoadingFinalizeNotice>(OnGameLoading);
            Communicator.AddCallback<GameLoadingRestartNotice>(OnGameLoading);
        }
        #endregion

        #region Class Methods
        public void ClearTaskTracking()
        {
            trackers.Clear();
        }

        private void AddTrackedTask(LoadingTaskTracker tracker)
        {
            int index = trackers.FindIndex(x =>
            {
                return x.Type == tracker.Type;
            });

            if (index == Consts.INDEX_NONE)
            {
                trackers.Add(tracker);
                return;
            }

            LoadingTaskTracker oldTracker = trackers[index];

            //This is weird, but to update the time marker properly, this is needed
            oldTracker.Status = tracker.Status;
            oldTracker.StepProgress = tracker.StepProgress;

            trackers[index] = oldTracker;
        }

        private bool TryUpdatingProvider()
        {
            LoadingProcessServiceProvider provider = GetFirstValidProvider();
            if (provider == null)
            {
                return false;
            }

            provider.Init(this);
            provider.UpdateProcess(trackers);

            ChangeStatus(LoadingProcessStatus.Loading);

            return true;
        }

        private void ChangeStatus(LoadingProcessStatus loadingStatus)
        {
            if (this.loadingStatus != loadingStatus)
            {
                switch (loadingStatus)
                {
                    case LoadingProcessStatus.Idle:
                    {
                        break;
                    }
                    case LoadingProcessStatus.Loading:
                    {
                        break;
                    }
                }
            }

            this.loadingStatus = loadingStatus;
        }

        private void OnLoadTaskMessage(TaskLoadingMessage message)
        {
            AddTrackedTask(message.trackerState);
        }

        private void OnGameLoadingNeedRestart(GameLoadingNeedRestart message)
        {
            IEnumerable<LoadingProcessServiceProvider> providers = GetAllProviders();
            foreach (LoadingProcessServiceProvider provider in providers)
            {
                provider.GameLoadingRestart(message);
            }
        }

        //Grab those steps to mark the debug step in the debugMenu
        private void OnGameLoading<T>(T message) where T : GameLoadingNotice
        {
            AddTrackedTask(new LoadingTaskTracker(message.GetType(), LoadingTrackingStatus.Finished));
        }

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        private void SetupDebugContent()
        {
            DebugMenuNotebook debugNotebook = new DebugMenuNotebook("LDNG", "Loading Process");
            EmptyMenuPage main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new TrackedTaskPage(this, "Loading tasks"));
            debugNotebook.Register();
        }
        #endregion Debug
        #endregion
    }
}

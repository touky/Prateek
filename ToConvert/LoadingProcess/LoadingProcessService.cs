namespace Assets.Prateek.ToConvert.LoadingProcess
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.DebugMenu.Content;
    using Assets.Prateek.ToConvert.DebugMenu.Pages;
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;
    using Assets.Prateek.ToConvert.Service;

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
            var index = trackers.FindIndex(x =>
            {
                return x.Type == tracker.Type;
            });

            if (index == Consts.INDEX_NONE)
            {
                trackers.Add(tracker);
                return;
            }

            var oldTracker = trackers[index];

            //This is weird, but to update the time marker properly, this is needed
            oldTracker.Status = tracker.Status;
            oldTracker.StepProgress = tracker.StepProgress;

            trackers[index] = oldTracker;
        }

        private bool TryUpdatingProvider()
        {
            var provider = GetFirstValidProvider();
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
            var providers = GetAllProviders();
            foreach (var provider in providers)
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
            var debugNotebook = new DebugMenuNotebook("LDNG", "Loading Process");
            var main          = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new TrackedTaskPage(this, "Loading tasks"));
            debugNotebook.Register();
        }
        #endregion Debug
        #endregion
    }
}

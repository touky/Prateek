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
    using Prateek.CommandFramework.Tools;
    using Prateek.TickableFramework.Code.Enums;

    public sealed class LoadingProcessDaemon : CommandReceiverDaemon<LoadingProcessDaemon, LoadingProcessServant>, IDebugMenuNotebookOwner
    {
        #region Fields
        private LoadingProcessStatus loadingStatus = LoadingProcessStatus.Idle;

        //This is intended to be a list to keep an ordered trace of the callers
        private List<LoadingTaskTracker> trackers = new List<LoadingTaskTracker>();
        #endregion

        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }

        public override void InitializeTickable()
        {
            base.InitializeTickable();

            SetupDebugContent();
        }

        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            if (!TryUpdatingProvider())
            {
                ChangeStatus(LoadingProcessStatus.Idle);
            }
        }
        
        #region Service
        #region Init
        protected override void OnAwake() { }
        #endregion
        #endregion

        #region Messaging
        public override void CommandReceived() { }

        protected override void SetupCommandReceiverCallback()
        {
            CommandReceiver.AddCallback<TaskLoadingCommand>(OnLoadTaskMessage);
            CommandReceiver.AddCallback<GameLoadingNeedRestart>(OnGameLoadingNeedRestart);

            CommandReceiver.AddCallback<GameLoadingPrerequisiteCommand>(OnGameLoading);
            CommandReceiver.AddCallback<GameLoadingGameplayCommand>(OnGameLoading);
            CommandReceiver.AddCallback<GameLoadingFinalizeCommand>(OnGameLoading);
            CommandReceiver.AddCallback<GameLoadingRestartCommand>(OnGameLoading);
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
            LoadingProcessServant servant = GetFirstAliveBranch();
            if (servant == null)
            {
                return false;
            }

            servant.Init(this);
            servant.UpdateProcess(trackers);

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

        private void OnLoadTaskMessage(TaskLoadingCommand command)
        {
            AddTrackedTask(command.trackerState);
        }

        private void OnGameLoadingNeedRestart(GameLoadingNeedRestart notice)
        {
            IEnumerable<LoadingProcessServant> providers = GetValidServants(true);
            foreach (LoadingProcessServant servant in providers)
            {
                servant.GameLoadingRestart(notice);
            }
        }

        //Grab those steps to mark the debug step in the debugMenu
        private void OnGameLoading<T>(T notice) where T : GameLoadingCommand
        {
            AddTrackedTask(new LoadingTaskTracker(notice.GetType(), LoadingTrackingStatus.Finished));
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

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
    using Prateek.NoticeFramework.Tools;

    public sealed class LoadingProcessDaemonCore : NoticeReceiverDaemonCore<LoadingProcessDaemonCore, LoadingProcessDaemonBranch>, IDebugMenuNotebookOwner
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
        public override void NoticeReceived() { }

        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<TaskLoadingNotice>(OnLoadTaskMessage);
            NoticeReceiver.AddCallback<GameLoadingNeedRestart>(OnGameLoadingNeedRestart);

            NoticeReceiver.AddCallback<GameLoadingPrerequisiteNotice>(OnGameLoading);
            NoticeReceiver.AddCallback<GameLoadingGameplayNotice>(OnGameLoading);
            NoticeReceiver.AddCallback<GameLoadingFinalizeNotice>(OnGameLoading);
            NoticeReceiver.AddCallback<GameLoadingRestartNotice>(OnGameLoading);
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
            LoadingProcessDaemonBranch branch = GetFirstAliveBranch();
            if (branch == null)
            {
                return false;
            }

            branch.Init(this);
            branch.UpdateProcess(trackers);

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

        private void OnLoadTaskMessage(TaskLoadingNotice notice)
        {
            AddTrackedTask(notice.trackerState);
        }

        private void OnGameLoadingNeedRestart(GameLoadingNeedRestart notice)
        {
            IEnumerable<LoadingProcessDaemonBranch> providers = GetValidBranches(true);
            foreach (LoadingProcessDaemonBranch branch in providers)
            {
                branch.GameLoadingRestart(notice);
            }
        }

        //Grab those steps to mark the debug step in the debugMenu
        private void OnGameLoading<T>(T notice) where T : GameLoadingNotice
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

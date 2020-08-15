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
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.CommandFramework;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public sealed class LoadingProcessDaemon
        : DaemonOverseer<LoadingProcessDaemon, LoadingProcessServant>
        , IDebugMenuNotebookOwner
        , ICommandReceiverOwner
        , IPreUpdateTickable
    {
        #region Fields
        private LoadingProcessStatus loadingStatus = LoadingProcessStatus.Idle;

        //This is intended to be a list to keep an ordered trace of the callers
        private List<LoadingTaskTracker> trackers = new List<LoadingTaskTracker>();
        #endregion

        protected override void OnAwake()
        {
            SetupDebugContent();
        }

        public void PreUpdate()
        {
            this.Get<ICommandReceiver>().ProcessReceivedCommands();

            if (!TryUpdatingProvider())
            {
                ChangeStatus(LoadingProcessStatus.Idle);
            }
        }

        #region Messaging
        public void DefineReceptionActions(ICommandReceiver receiver)
        {
            receiver.SetActionFor<TaskLoadingCommand>(OnLoadTaskMessage);
            receiver.SetActionFor<GameLoadingNeedRestart>(OnGameLoadingNeedRestart);

            receiver.SetActionFor<GameLoadingPrerequisiteCommand>(OnGameLoading);
            receiver.SetActionFor<GameLoadingGameplayCommand>(OnGameLoading);
            receiver.SetActionFor<GameLoadingFinalizeCommand>(OnGameLoading);
            receiver.SetActionFor<GameLoadingRestartCommand>(OnGameLoading);
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
            var servant = FirstAliveServant;
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
            foreach (var servant in AllServants)
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
        [Conditional("PRATEEK_DEBUG")]
        private void SetupDebugContent()
        {
            DebugMenuNotebook debugNotebook = new DebugMenuNotebook("LDNG", "Loading Process");
            EmptyMenuPage main = new EmptyMenuPage("MAIN");
            debugNotebook.AddPagesWithParent(main, new TrackedTaskPage(this, "Loading tasks"));
            debugNotebook.Register();
        }

        public int Priority(IPriority<IApplicationFeedbackTickable> type)
        {
            throw new System.NotImplementedException();
        }

        public int Priority(IPriority<IPreUpdateTickable> type)
        {
            throw new System.NotImplementedException();
        }
        #endregion Debug
        #endregion
    }
}

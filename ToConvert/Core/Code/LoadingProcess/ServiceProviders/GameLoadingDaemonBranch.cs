namespace Mayfair.Core.Code.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.LoadingProcess.StateMachine;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public class GameLoadingDaemonBranch : LoadingProcessDaemonBranch
    {
        #region Fields
        private FiniteStateMachine<LoadingProcessTrigger> stateMachine;
        #endregion

        #region Properties
        public override int Priority
        {
            get { return Consts.SECOND_ITEM; }
        }
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (IsAlive && stateMachine != null)
            {
                stateMachine.Advance();
            }
        }
        #endregion

        #region Class Methods
        public override void UpdateProcess(List<LoadingTaskTracker> trackers)
        {
            if (stateMachine != null)
            {
                LoadingTrackingStatus trackerStatus = LoadingTrackingStatus.Finished;
                foreach (LoadingTaskTracker tracker in trackers)
                {
                    trackerStatus = (LoadingTrackingStatus) Mathf.Min((int) trackerStatus, (int) tracker.Status);
                }

                stateMachine.Trigger(trackerStatus);
            }
        }

        public override void GameLoadingRestart(GameLoadingNeedRestart notice)
        {
            stateMachine.Trigger(notice);
            stateMachine.Advance();
        }

        protected override void InternalInit(LoadingProcessDaemonCore daemonCore)
        {
            LoadingIdleState idle = new LoadingIdleState();

            TaskTrackingClearState taskClear = new TaskTrackingClearState(daemonCore);

            //Send notice
            NoticeState<GameLoadingPrerequisiteNotice> prereqNotice = new NoticeState<GameLoadingPrerequisiteNotice>(daemonCore.NoticeReceiver, Consts.WAIT_5_FRAMES);
            LoadingIdleState prereqIdle = new LoadingIdleState();

            //Send notice
            NoticeState<GameLoadingGameplayNotice> gameplayNotice = new NoticeState<GameLoadingGameplayNotice>(daemonCore.NoticeReceiver, Consts.WAIT_5_FRAMES);
            LoadingIdleState gameplayIdle = new LoadingIdleState();

            //Send notice
            NoticeState<GameLoadingFinalizeNotice> finalizeNotice = new NoticeState<GameLoadingFinalizeNotice>(daemonCore.NoticeReceiver, Consts.WAIT_5_FRAMES);
            LoadingIdleState finalizeIdle = new LoadingIdleState();

            //Send notice
            NoticeState<GameLoadingFinishedNotice> finishedNotice = new NoticeState<GameLoadingFinishedNotice>(daemonCore.NoticeReceiver);

            //Loading end
            LoadingStatusState<LoadingProcessTrigger> endLoading = new LoadingStatusState<LoadingProcessTrigger>(this, true);

            //Send notice
            LoadingStatusState<LoadingProcessTrigger> restartLoading = new LoadingStatusState<LoadingProcessTrigger>(this, false);
            TaskTrackingClearState restartClear = new TaskTrackingClearState(daemonCore);
            NoticeState<GameLoadingRestartNotice> restartNotice = new NoticeState<GameLoadingRestartNotice>(daemonCore.NoticeReceiver, Consts.WAIT_5_FRAMES);
            LoadingIdleState restartIdle = new LoadingIdleState();

            new LoadingBoolTransition().From(idle).To(taskClear);
            new AutoTriggerTransition().From(taskClear).To(prereqNotice);

            new LoadingBoolTransition().From(prereqNotice).To(prereqIdle);
            new LoadingStatusTransition(LoadingTrackingStatus.HasLoadedPrerequisite).From(prereqIdle).To(gameplayNotice);

            new LoadingBoolTransition().From(gameplayNotice).To(gameplayIdle);
            new LoadingStatusTransition(LoadingTrackingStatus.HasLoadedPrerequisite).From(gameplayIdle).To(finalizeNotice);

            new LoadingBoolTransition().From(finalizeNotice).To(finalizeIdle);
            new LoadingStatusTransition(LoadingTrackingStatus.HasLoadedPrerequisite).From(finalizeIdle).To(finishedNotice);

            new LoadingBoolTransition().From(finishedNotice).To(endLoading);

            new GameLoadingRestartTransition().From(endLoading).To(restartLoading);
            new AutoTriggerTransition().From(restartLoading).To(restartClear);
            new AutoTriggerTransition().From(restartClear).To(restartNotice);
            new LoadingBoolTransition().From(restartNotice).To(restartIdle);
            new LoadingStatusTransition(LoadingTrackingStatus.Finished).From(restartIdle).To(taskClear);

            stateMachine = new FiniteStateMachine<LoadingProcessTrigger>(idle);
            stateMachine.Advance();
            stateMachine.Trigger(true);
        }
        #endregion
    }
}

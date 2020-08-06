namespace Mayfair.Core.Code.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.LoadingProcess.Enums;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.LoadingProcess.StateMachine;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;
    using Prateek.Runtime.TickableFramework.Enums;
    using UnityEngine;

    public class GameLoadingServant : LoadingProcessServant
    {
        #region Fields
        private StandardStateMachine<LoadingProcessTrigger> stateMachine;
        #endregion

        #region Properties
        public override int Priority
        {
            get { return Consts.SECOND_ITEM; }
        }

        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }
        #endregion

        #region Class Methods
        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            if (IsAlive && stateMachine != null)
            {
                stateMachine.Step();
            }
        }

        public override void UpdateProcess(List<LoadingTaskTracker> trackers)
        {
            if (stateMachine != null)
            {
                var trackerStatus = LoadingTrackingStatus.Finished;
                foreach (var tracker in trackers)
                {
                    trackerStatus = (LoadingTrackingStatus) Mathf.Min((int) trackerStatus, (int) tracker.Status);
                }

                stateMachine.Trigger(trackerStatus);
            }
        }

        public override void GameLoadingRestart(GameLoadingNeedRestart notice)
        {
            stateMachine.Trigger(notice);
            stateMachine.Step();
        }

        protected override void InternalInit(LoadingProcessDaemon daemonCore)
        {
            var idle = new LoadingIdleState();

            var taskClear = new TaskTrackingClearState(daemonCore);

            //Send notice
            var prereqNotice = new NoticeState<GameLoadingPrerequisiteCommand>(daemonCore.CommandReceiver, Consts.WAIT_5_FRAMES);
            var prereqIdle   = new LoadingIdleState();

            //Send notice
            var gameplayNotice = new NoticeState<GameLoadingGameplayCommand>(daemonCore.CommandReceiver, Consts.WAIT_5_FRAMES);
            var gameplayIdle   = new LoadingIdleState();

            //Send notice
            var finalizeNotice = new NoticeState<GameLoadingFinalizeCommand>(daemonCore.CommandReceiver, Consts.WAIT_5_FRAMES);
            var finalizeIdle   = new LoadingIdleState();

            //Send notice
            var finishedNotice = new NoticeState<GameLoadingFinishedCommand>(daemonCore.CommandReceiver);

            //Loading end
            var endLoading = new LoadingStatusState<LoadingProcessTrigger>(this, true);

            //Send notice
            var restartLoading = new LoadingStatusState<LoadingProcessTrigger>(this, false);
            var restartClear   = new TaskTrackingClearState(daemonCore);
            var restartNotice  = new NoticeState<GameLoadingRestartCommand>(daemonCore.CommandReceiver, Consts.WAIT_5_FRAMES);
            var restartIdle    = new LoadingIdleState();

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

            stateMachine = new StandardStateMachine<LoadingProcessTrigger>(idle);
            stateMachine.Step();
            stateMachine.Trigger(true);
        }
        #endregion
    }
}

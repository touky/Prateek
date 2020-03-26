namespace Assets.Prateek.ToConvert.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.LoadingProcess.Enums;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;
    using Assets.Prateek.ToConvert.LoadingProcess.StateMachine;
    using Assets.Prateek.ToConvert.StateMachines.FSM;
    using UnityEngine;

    public class GameLoadingServiceProvider : LoadingProcessServiceProvider
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
            if (IsValid && stateMachine != null)
            {
                stateMachine.Advance();
            }
        }
        #endregion

        #region Class Methods
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<LoadingProcessService, LoadingProcessServiceProvider>(this);
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

        public override void GameLoadingRestart(GameLoadingNeedRestart message)
        {
            stateMachine.Trigger(message);
            stateMachine.Advance();
        }

        protected override void InternalInit(LoadingProcessService service)
        {
            var idle = new LoadingIdleState();

            var taskClear = new TaskTrackingClearState(service);

            //Send notice
            var prereqNotice = new NoticeState<GameLoadingPrerequisiteNotice>(service.Communicator, Consts.WAIT_5_FRAMES);
            var prereqIdle   = new LoadingIdleState();

            //Send notice
            var gameplayNotice = new NoticeState<GameLoadingGameplayNotice>(service.Communicator, Consts.WAIT_5_FRAMES);
            var gameplayIdle   = new LoadingIdleState();

            //Send notice
            var finalizeNotice = new NoticeState<GameLoadingFinalizeNotice>(service.Communicator, Consts.WAIT_5_FRAMES);
            var finalizeIdle   = new LoadingIdleState();

            //Send notice
            var finishedNotice = new NoticeState<GameLoadingFinishedNotice>(service.Communicator);

            //Loading end
            var endLoading = new LoadingStatusState<LoadingProcessTrigger>(this, true);

            //Send notice
            var restartLoading = new LoadingStatusState<LoadingProcessTrigger>(this, false);
            var restartClear   = new TaskTrackingClearState(service);
            var restartNotice  = new NoticeState<GameLoadingRestartNotice>(service.Communicator, Consts.WAIT_5_FRAMES);
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

            stateMachine = new FiniteStateMachine<LoadingProcessTrigger>(idle);
            stateMachine.Advance();
            stateMachine.Trigger(true);
        }
        #endregion
    }
}

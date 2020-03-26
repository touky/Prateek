namespace Assets.Prateek.ToConvert.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.GameScene;
    using Assets.Prateek.ToConvert.LoadingProcess.Messages;
    using Assets.Prateek.ToConvert.Localization;
    using Assets.Prateek.ToConvert.StateMachines.FSM;
    using Assets.Prateek.ToConvert.StateMachines.FSM.BoolTrigger;
    using UnityEngine;

    public class GameInitServiceProvider : LoadingProcessServiceProvider
    {
        #region Settings
        [SerializeField]
        private string servicesScene = "InitServices";

        [SerializeField]
        private string gameScene = "InitGame";
        #endregion

        #region Fields
        private FiniteStateMachine<bool> stateMachine;
        #endregion

        #region Properties
        public override int Priority
        {
            get { return Consts.FIRST_ITEM; }
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

        protected override void InternalInit(LoadingProcessService service)
        {
            if (!LocalizationService.SetLanguage(SystemLanguage.English, true))
            {
                throw new MissingMinimalLocalizationException(SystemLanguage.English);
            }

            IdleBoolState idle          = new IdleBoolState();
            var           loaderService = new SceneLoaderState(servicesScene);
            var           activator     = new GameInitActivatorState();
            var           loaderGame    = new SceneLoaderState(gameScene);
            var           loadingStatus = new LoadingStatusState<bool>(this, true);

            new BoolTriggerTransition().From(idle).To(loaderService);
            new BoolTriggerTransition().From(loaderService).To(activator);
            new BoolTriggerTransition().From(activator).To(loaderGame);
            new BoolTriggerTransition().From(loaderGame).To(loadingStatus);

            stateMachine = new FiniteStateMachine<bool>(idle);
            stateMachine.Advance();
            stateMachine.Trigger(true);
        }

        public override void UpdateProcess(List<LoadingTaskTracker> trackers) { }

        public override void GameLoadingRestart(GameLoadingNeedRestart message)
        {
            //Do nothing
        }
        #endregion
    }
}

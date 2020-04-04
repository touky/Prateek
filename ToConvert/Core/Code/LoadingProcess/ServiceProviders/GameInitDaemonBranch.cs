using Mayfair.Core.Code.Localization;

namespace Mayfair.Core.Code.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameScene;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.StateMachines.FSM.BoolTrigger;
    using Mayfair.Core.Code.Utils;
    using UnityEngine;

    public class GameInitDaemonBranch : LoadingProcessDaemonBranch
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
            if (IsAlive && stateMachine != null)
            {
                stateMachine.Advance();
            }
        }
        #endregion

        #region Class Methods
        protected override void InternalInit(LoadingProcessDaemonCore daemonCore)
        {
            if (!LocalizationDaemonCore.SetLanguage(SystemLanguage.English, true))
            {
                throw new MissingMinimalLocalizationException(SystemLanguage.English);
            }

            IdleBoolState idle = new IdleBoolState();
            SceneLoaderState loaderService = new SceneLoaderState(servicesScene);
            GameInitActivatorState activator = new GameInitActivatorState();
            SceneLoaderState loaderGame = new SceneLoaderState(gameScene);
            LoadingStatusState<bool> loadingStatus = new LoadingStatusState<bool>(this, true);

            new BoolTriggerTransition().From(idle).To(loaderService);
            new BoolTriggerTransition().From(loaderService).To(activator);
            new BoolTriggerTransition().From(activator).To(loaderGame);
            new BoolTriggerTransition().From(loaderGame).To(loadingStatus);

            stateMachine = new FiniteStateMachine<bool>(idle);
            stateMachine.Advance();
            stateMachine.Trigger(true);
        }

        public override void UpdateProcess(List<LoadingTaskTracker> trackers) { }

        public override void GameLoadingRestart(GameLoadingNeedRestart notice)
        {
            //Do nothing
        }
        #endregion
    }
}

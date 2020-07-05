namespace Mayfair.Core.Code.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameScene;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Localization;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.StateMachines.FSM.BoolTrigger;
    using Mayfair.Core.Code.Utils;
    using Prateek.TickableFramework.Code.Enums;
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
                stateMachine.Advance();
            }
        }

        protected override void InternalInit(LoadingProcessDaemonCore daemonCore)
        {
            if (!LocalizationDaemonCore.SetLanguage(SystemLanguage.English, true))
            {
                throw new MissingMinimalLocalizationException(SystemLanguage.English);
            }

            var idle          = new IdleBoolState();
            var loaderService = new SceneLoaderState(servicesScene);
            var activator     = new GameInitActivatorState();
            var loaderGame    = new SceneLoaderState(gameScene);
            var loadingStatus = new LoadingStatusState<bool>(this, true);

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

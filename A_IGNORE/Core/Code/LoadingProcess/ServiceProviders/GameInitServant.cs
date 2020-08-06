namespace Mayfair.Core.Code.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameScene;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Localization;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;
    using Prateek.Runtime.TickableFramework.Enums;
    using UnityEngine;

    public class GameInitServant : LoadingProcessServant
    {
        #region Settings
        [SerializeField]
        private string servicesScene = "InitServices";

        [SerializeField]
        private string gameScene = "InitGame";
        #endregion

        #region Fields
        private StandardStateMachine<bool> stateMachine;
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
                stateMachine.Step();
            }
        }

        protected override void InternalInit(LoadingProcessDaemon daemonCore)
        {
            if (!LocalizationDaemon.SetLanguage(SystemLanguage.English, true))
            {
                throw new MissingMinimalLocalizationException(SystemLanguage.English);
            }

            var idle          = new ToggleIdleState();
            var loaderService = new SceneLoaderState(servicesScene);
            var activator     = new GameInitActivatorState();
            var loaderGame    = new SceneLoaderState(gameScene);
            var loadingStatus = new LoadingStatusState<bool>(this, true);

            new ToggleTransition().From(idle).To(loaderService);
            new ToggleTransition().From(loaderService).To(activator);
            new ToggleTransition().From(activator).To(loaderGame);
            new ToggleTransition().From(loaderGame).To(loadingStatus);

            stateMachine = new StandardStateMachine<bool>(idle);
            stateMachine.Step();
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

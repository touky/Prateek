namespace Mayfair.Core.Code.LoadingProcess.ServiceProviders
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.GameScene;
    using Mayfair.Core.Code.LoadingProcess.Messages;
    using Mayfair.Core.Code.Localization;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Mayfair.Core.Code.Utils;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    using Prateek.Runtime.TickableFramework.Interfaces;
    using UnityEngine;

    public class GameInitServant
        : LoadingProcessServant
        , IPreUpdateTickable
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
        public override int DefaultPriority
        {
            get { return Consts.FIRST_ITEM; }
        }
        #endregion

        #region Class Methods
        public void PreUpdate()
        {
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

        public int Priority(IPriority<IApplicationFeedbackTickable> type)
        {
            throw new System.NotImplementedException();
        }

        public int Priority(IPriority<IPreUpdateTickable> type)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}

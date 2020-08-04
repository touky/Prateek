namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.A_TODO.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.TickableFramework.Enums;

    public sealed class VisualResourceDaemon : ContentAccessDaemon<VisualResourceDaemon, VisualResourceServant>
    {
        #region Fields
        private DebugMenuNotebook debugNotebook;
        private DebugMenuPage debugMainPage;
        #endregion

        #region Properties
        protected override ServiceProviderUsageRuleType ServiceProviderUsageRule
        {
            get { return ServiceProviderUsageRuleType.UseAllValid; }
        }
        #endregion
        
        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }

        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            PerformProviderAction(ServiceProviderUsageRule,
                                  servant =>
                                  {
                                      servant.RefreshPending();
                                  });
        }

        #region Service
        protected override void OnAwake()
        {
            SetupDebugContent();
        }
        #endregion

        #region Messaging
        public override void DefineCommandReceiverActions()
        {
            base.DefineCommandReceiverActions();

            CommandReceiver.SetActionFor<VisualResourceDirectCommand>(OnVisualResourceMessage);
        }
        #endregion

        #region Class Methods
        protected override void OnServantRegistered(VisualResourceServant servant)
        {
            base.OnServantRegistered(servant);

            servant.SetupDebugContent(debugNotebook, debugMainPage);
        }

        private void OnVisualResourceMessage(VisualResourceDirectCommand command)
        {
            foreach (var servant in AllAliveServants)
            {
                if (!command.AllowTransfer(servant))
                {
                    continue;
                }

                servant.OnVisualResourceMessage(command);
            }
        }

        #region Debug
        [Conditional("PRATEEK_DEBUG")]
        public void SetupDebugContent()
        {
            debugNotebook = new DebugMenuNotebook("VSLR", "Visual Resources");
            debugMainPage = new EmptyMenuPage("MAIN");
            debugNotebook.Register();
        }
        #endregion
        #endregion
    }
}

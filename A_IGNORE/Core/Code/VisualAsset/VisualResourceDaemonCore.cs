namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Mayfair.Core.Code.VisualAsset.Providers;
    using Prateek.TickableFramework.Code.Enums;

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
        public override void CommandReceived() { }

        protected override void SetupCommandReceiverCallback()
        {
            base.SetupCommandReceiverCallback();

            CommandReceiver.AddCallback<VisualResourceDirectCommand>(OnVisualResourceMessage);
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
            IEnumerable<VisualResourceServant> providers = GetValidServants();
            foreach (VisualResourceServant servant in providers)
            {
                if (!command.AllowTransfer(servant))
                {
                    continue;
                }

                servant.OnVisualResourceMessage(command);
            }
        }

        #region Debug
        [Conditional("NVIZZIO_DEV")]
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

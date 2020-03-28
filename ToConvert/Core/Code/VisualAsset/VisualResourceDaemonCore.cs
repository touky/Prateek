namespace Mayfair.Core.Code.VisualAsset
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Resources;
    using Mayfair.Core.Code.VisualAsset.Messages;
    using Mayfair.Core.Code.VisualAsset.Providers;

    public sealed class VisualResourceDaemonCore : ResourceDependentDaemonCore<VisualResourceDaemonCore, VisualResourceDaemonBranch>
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

        #region Service
        protected override void OnAwake()
        {
            SetupDebugContent();
        }
        #endregion

        #region Messaging
        public override void MessageReceived() { }

        protected override void SetupCommunicatorCallback()
        {
            base.SetupCommunicatorCallback();

            Communicator.AddCallback<VisualResourceDirectMessage>(OnVisualResourceMessage);
        }
        #endregion

        #region Class Methods
        protected override void OnBranchRegistered(VisualResourceDaemonBranch branch)
        {
            base.OnBranchRegistered(branch);

            branch.SetupDebugContent(debugNotebook, debugMainPage);
        }

        private void OnVisualResourceMessage(VisualResourceDirectMessage message)
        {
            IEnumerable<VisualResourceDaemonBranch> providers = GetValidBranches();
            foreach (VisualResourceDaemonBranch branch in providers)
            {
                if (!message.AllowTransfer(branch))
                {
                    continue;
                }

                branch.OnVisualResourceMessage(message);
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

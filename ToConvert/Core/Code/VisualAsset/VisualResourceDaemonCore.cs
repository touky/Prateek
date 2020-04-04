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
        public override void NoticeReceived() { }

        protected override void SetupNoticeReceiverCallback()
        {
            base.SetupNoticeReceiverCallback();

            NoticeReceiver.AddCallback<VisualResourceDirectNotice>(OnVisualResourceMessage);
        }
        #endregion

        #region Class Methods
        protected override void OnBranchRegistered(VisualResourceDaemonBranch branch)
        {
            base.OnBranchRegistered(branch);

            branch.SetupDebugContent(debugNotebook, debugMainPage);
        }

        private void OnVisualResourceMessage(VisualResourceDirectNotice notice)
        {
            IEnumerable<VisualResourceDaemonBranch> providers = GetValidBranches();
            foreach (VisualResourceDaemonBranch branch in providers)
            {
                if (!notice.AllowTransfer(branch))
                {
                    continue;
                }

                branch.OnVisualResourceMessage(notice);
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

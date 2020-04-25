namespace Mayfair.Core.Code.Resources
{
    using System;
    using Mayfair.Core.Code.Resources.Messages;
    using Prateek.NoticeFramework.Tools;

    public abstract class ContentAccessDaemonCore<TDaemonCore, TDaemonBranch>
        : NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonCore : NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : ContentAccessDaemonBranch<TDaemonCore, TDaemonBranch>
    {
        #region ServiceProviderUsageRuleType enum
        protected enum ServiceProviderUsageRuleType
        {
            Nothing,

            UseFirstValid,
            UseAllValid
        }
        #endregion

        #region Properties
        protected abstract ServiceProviderUsageRuleType ServiceProviderUsageRule { get; }
        #endregion

        #region Class Methods
        public override void InitializeTickable()
        {
            base.InitializeTickable();

            RegisterToResourceService();
        }

        #region Messaging
        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<ResourcesHaveChangedResponse>(OnResourceUpdateCallback);
        }
        #endregion

        private void RegisterToResourceService()
        {
            PerformProviderAction(ServiceProviderUsageRule,
                                  branch =>
                                  {
                                      NoticeReceiver.Send(branch.GetResourceChangeRequest(NoticeReceiver));
                                  });
        }

        private void OnResourceUpdateCallback(ResourcesHaveChangedResponse notice)
        {
            PerformProviderAction(ServiceProviderUsageRule,
                                  branch =>
                                  {
                                      branch.OnResourceChanged(Instance, notice);
                                  });
        }

        protected void PerformProviderAction(ServiceProviderUsageRuleType rule, Action<TDaemonBranch> action)
        {
            if (rule == ServiceProviderUsageRuleType.UseAllValid)
            {
                var providers = GetValidBranches();
                foreach (var branch in providers)
                {
                    action(branch);
                }
            }
            else
            {
                var branch = GetFirstAliveBranch();
                if (branch != null)
                {
                    action(branch);
                }
            }
        }
        #endregion
    }
}

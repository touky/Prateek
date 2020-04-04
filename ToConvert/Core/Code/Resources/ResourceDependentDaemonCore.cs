namespace Mayfair.Core.Code.Resources
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Resources.Messages;
    using Prateek.NoticeFramework.Tools;

    public abstract class ResourceDependentDaemonCore<TDaemonCore, TDaemonBranch> : NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonCore : NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : ResourceDependentDaemonBranch<TDaemonCore, TDaemonBranch>
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

        #region Unity Methods
        protected virtual void Start()
        {
            RegisterToResourceService();
        }
        #endregion

        #region Messaging
        protected override void SetupNoticeReceiverCallback()
        {
            NoticeReceiver.AddCallback<ResourcesHaveChangedResponse>(OnResourceUpdateCallback);
        }
        #endregion

        #region Class Methods
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
                IEnumerable<TDaemonBranch> providers = GetValidBranches();
                foreach (TDaemonBranch branch in providers)
                {
                    action(branch);
                }
            }
            else
            {
                TDaemonBranch branch = GetFirstAliveBranch();
                if (branch != null)
                {
                    action(branch);
                }
            }
        }
        #endregion
    }
}

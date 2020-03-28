namespace Mayfair.Core.Code.Resources
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Utils.Helpers;

    public abstract class ResourceDependentDaemonCore<TDaemonCore, TDaemonBranch> : DaemonCoreCommunicator<TDaemonCore, TDaemonBranch>
        where TDaemonCore : DaemonCoreCommunicator<TDaemonCore, TDaemonBranch>
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
        protected override void SetupCommunicatorCallback()
        {
            Communicator.AddCallback<ResourcesHaveChangedResponse>(OnResourceUpdateCallback);
        }
        #endregion

        #region Class Methods
        private void RegisterToResourceService()
        {
            PerformProviderAction(ServiceProviderUsageRule,
                 branch =>
                 {
                     Communicator.Send(branch.GetResourceChangeRequest(Communicator));
                 });
        }

        private void OnResourceUpdateCallback(ResourcesHaveChangedResponse message)
        {
            PerformProviderAction(ServiceProviderUsageRule,
                 branch =>
                 {
                     branch.OnResourceChanged(Instance, message);
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

namespace Prateek.A_TODO.Runtime.AppContentFramework.Daemons
{
    using System;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;

    public abstract class ContentAccessDaemon<TDaemon, TServant>
        : CommandReceiverDaemon<TDaemon, TServant>
        where TDaemon : CommandReceiverDaemon<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
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
        protected override void SetupCommandReceiverCallback()
        {
            CommandReceiver.AddCallback<ResourcesHaveChangedResponse>(OnResourceUpdateCallback);
        }
        #endregion

        private void RegisterToResourceService()
        {
            PerformProviderAction(ServiceProviderUsageRule,
                                  servant =>
                                  {
                                      CommandReceiver.Send(servant.GetResourceChangeRequest(CommandReceiver));
                                  });
        }

        private void OnResourceUpdateCallback(ResourcesHaveChangedResponse notice)
        {
            PerformProviderAction(ServiceProviderUsageRule,
                                  servant =>
                                  {
                                      servant.OnResourceChanged(Instance, notice);
                                  });
        }

        protected void PerformProviderAction(ServiceProviderUsageRuleType rule, Action<TServant> action)
        {
            if (rule == ServiceProviderUsageRuleType.UseAllValid)
            {
                foreach (var servant in AllAliveServants)
                {
                    action(servant);
                }
            }
            else
            {
                var servant = FirstAliveServant;
                if (servant != null)
                {
                    action(servant);
                }
            }
        }
        #endregion
    }
}

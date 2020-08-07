namespace Prateek.A_TODO.Runtime.AppContentFramework.Daemons
{
    using System;
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;

    public abstract class ContentAccessDaemon<TDaemon, TServant>
        : ReceiverDaemonOverseer<TDaemon, TServant>
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
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
        protected override void OnAwake()
        {
            base.OnAwake();

            RegisterToResourceService();
        }

        #region Messaging
        public override void DefineCommandReceiverActions()
        {
            CommandReceiver.SetActionFor<ResourcesHaveChangedResponse>(OnResourceUpdateCallback);
        }
        #endregion

        private void RegisterToResourceService()
        {
            //todo PerformProviderAction(ServiceProviderUsageRule,
            //todo                       servant =>
            //todo                       {
            //todo                           CommandReceiver.Send(servant.GetResourceChangeRequest(CommandReceiver));
            //todo                       });
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

namespace Assets.Prateek.ToConvert.Resources
{
    using System;
    using Assets.Prateek.ToConvert.Resources.Messages;
    using Assets.Prateek.ToConvert.Service;

    public abstract class ResourceDependentService<TService, TProvider> : ServiceCommunicatorBehaviour<TService, TProvider>
        where TService : ServiceCommunicatorBehaviour<TService, TProvider>
        where TProvider : ResourceDependentServiceProvider<TService, TProvider>
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
                                  provider =>
                                  {
                                      Communicator.Send(provider.GetResourceChangeRequest(Communicator));
                                  });
        }

        private void OnResourceUpdateCallback(ResourcesHaveChangedResponse message)
        {
            PerformProviderAction(ServiceProviderUsageRule,
                                  provider =>
                                  {
                                      provider.OnResourceChanged(Instance, message);
                                  });
        }

        protected void PerformProviderAction(ServiceProviderUsageRuleType rule, Action<TProvider> action)
        {
            if (rule == ServiceProviderUsageRuleType.UseAllValid)
            {
                var providers = GetAllValidProviders();
                foreach (var provider in providers)
                {
                    action(provider);
                }
            }
            else
            {
                var provider = GetFirstValidProvider();
                if (provider != null)
                {
                    action(provider);
                }
            }
        }
        #endregion
    }
}

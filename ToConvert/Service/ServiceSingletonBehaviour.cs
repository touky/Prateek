namespace Assets.Prateek.ToConvert.Service
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Priority;
    using Assets.Prateek.ToConvert.Service.Interfaces;

    public abstract class ServiceSingletonBehaviour<TService, TProvider> : AbstractSingleton<TService>, IService<TProvider>
        where TService : ServiceSingletonBehaviour<TService, TProvider>
        where TProvider : Interfaces.IServiceProvider
    {
        #region Fields
        private List<TProvider> providers = new List<TProvider>();
        #endregion

        #region Properties
        protected IReadOnlyList<TProvider> Providers
        {
            get { return providers; }
        }
        #endregion

        #region Class Methods
        internal static void ReceiveIdentificationFor(ServiceIdentificationAction identificationAction, TProvider provider)
        {
            IService<TProvider> instance = Instance;

            //This will only happen when OnApplicationQuit has been called
            if (instance == null)
            {
                return;
            }

            switch (identificationAction)
            {
                case ServiceIdentificationAction.Register:
                {
                    instance.Register(provider);
                    break;
                }
                case ServiceIdentificationAction.Unregister:
                {
                    instance.Unregister(provider);
                    break;
                }
                default:
                {
                    throw new Exception($"{provider.GetType().Name} sent idenfication without the action setup.");
                }
            }
        }

        public TProvider GetFirstValidProvider()
        {
            foreach (var provider in providers)
            {
                if (provider.IsValid)
                {
                    return provider;
                }
            }

            return default;
        }

        public IEnumerable<TProvider> GetAllValidProviders()
        {
            return new ProviderEnumerable<TProvider>(providers);
        }

        public IEnumerable<TProvider> GetAllProviders()
        {
            return new ProviderEnumerable<TProvider>(providers, true);
        }

        protected virtual void OnRegister(TProvider provider) { }
        protected virtual void OnUnregister(TProvider provider) { }
        #endregion

        #region IService<TProvider> Members
        void IService<TProvider>.Register(TProvider provider)
        {
            if (providers.Contains(provider))
            {
                return;
            }

            providers.Add(provider);
            providers.SortWithPriorities();

            OnRegister(provider);
        }

        void IService<TProvider>.Unregister(TProvider provider)
        {
            if (!providers.Contains(provider))
            {
                return;
            }

            providers.Remove(provider);
            providers.SortWithPriorities();

            OnUnregister(provider);
        }
        #endregion
    }
}

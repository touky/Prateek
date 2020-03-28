namespace Mayfair.Core.Code.Service
{
    using Mayfair.Core.Code.Service.Interfaces;

    public abstract class ServiceProvider : IServiceProvider
    {
        #region Fields
        private ServiceIdentificationAction identificationAction = ServiceIdentificationAction.None;
        #endregion

        #region Class Methods
        public virtual void Startup()
        {
            RequestIdentificationFor(ServiceIdentificationAction.Register);
        }

        public virtual void Shutdown()
        {
            RequestIdentificationFor(ServiceIdentificationAction.Unregister);
        }
        #endregion

        #region Identification request
        protected abstract void OnIdentificationRequested();

        protected void SendIdentificationFor<TService, TProvider>(TProvider provider)
            where TService : ServiceSingletonBehaviour<TService, TProvider>
            where TProvider : ServiceProvider
        {
            ServiceSingletonBehaviour<TService, TProvider>.ReceiveIdentificationFor(identificationAction, provider);
        }

        private void RequestIdentificationFor(ServiceIdentificationAction mode)
        {
            identificationAction = mode;
            {
                OnIdentificationRequested();
            }
            identificationAction = ServiceIdentificationAction.None;
        }
        #endregion

        #region IServiceProvider Members
        public abstract bool IsValid { get; }
        public abstract int Priority { get; }
        #endregion
    }
}

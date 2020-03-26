namespace Assets.Prateek.ToConvert.Service
{
    using Assets.Prateek.ToConvert.Service.Interfaces;
    using UnityEngine;

    public abstract class ServiceProviderBehaviour : MonoBehaviour, IServiceProvider
    {
        #region Fields
        private ServiceIdentificationAction identificationAction = ServiceIdentificationAction.None;
        #endregion

        #region Properties
        public abstract bool IsProviderValid { get; }
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            RequestIdentificationFor(ServiceIdentificationAction.Register);
        }

        protected virtual void OnDestroy()
        {
            RequestIdentificationFor(ServiceIdentificationAction.Unregister);
        }
        #endregion

        #region IServiceProvider Members
        public bool IsValid
        {
            get { return enabled && IsProviderValid; }
        }

        public abstract int Priority { get; }
        #endregion

        #region Identification request
        protected abstract void OnIdentificationRequested();

        protected void SendIdentificationFor<TService, TProvider>(TProvider provider)
            where TService : ServiceSingletonBehaviour<TService, TProvider>
            where TProvider : ServiceProviderBehaviour
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
    }
}

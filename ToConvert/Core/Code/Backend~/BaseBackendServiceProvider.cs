namespace Mayfair.Core.Code.Backend
{
    using Service;
    using ServiceProviders;

    public abstract class BaseBackendServiceProvider : ServiceProviderBehaviour
    {
        public abstract void Login(LoginSuccessCallbackHandler onLoginSuccess, FailureCallbackHandler onLoginFailure);
        public abstract void CreateServerTimeProvider();

        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<BackendService, BaseBackendServiceProvider>(this);
        }
    }
}
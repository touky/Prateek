namespace Mayfair.Core.Code.Backend.ServiceProviders.PlayFab
{
    using global::PlayFab;
    using Requests;
    using TimeService.ServiceProviders;
    using UnityEngine;

    public class PlayFabBackendServiceProvider : BaseBackendServiceProvider
    {
        private PlayFabRequestManager requestManager;

        public override bool IsProviderValid
        {
            get => true;
        }

        public override int Priority
        {
            get => 0;
        }

        public PlayFabBackendServiceProvider()
        {
            requestManager = new PlayFabRequestManager();
        }

        public override void Login(LoginSuccessCallbackHandler onLoginSuccess, FailureCallbackHandler onLoginFailure)
        {
            PlayFabLoginRequest request = new LoginWithCustomIdRequest(SystemInfo.deviceUniqueIdentifier, onLoginSuccess, onLoginFailure, PlayFabClientAPI.LoginWithCustomID);
            requestManager.QueueRequest(request);
        }

        public override void CreateServerTimeProvider()
        {
            // This is not typical for PlayFab requests and should not be used as an example.
            GetServerTimeRequest getTimeRequest = new GetServerTimeRequest();
            getTimeRequest.Dispatch(serverTime =>
            {
                PlayFabTimeServiceProvider timeServiceProvider = new PlayFabTimeServiceProvider(serverTime);
                timeServiceProvider.Startup();
            });
        }
    }
}
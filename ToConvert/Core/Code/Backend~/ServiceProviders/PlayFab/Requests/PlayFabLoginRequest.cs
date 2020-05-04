namespace Mayfair.Core.Code.Backend.ServiceProviders.PlayFab.Requests
{
    using global::PlayFab.ClientModels;
    using UnityEngine.Assertions;

    public abstract class PlayFabLoginRequest : BasePlayFabRequest<LoginWithCustomIDRequest, LoginResult>
    {
        private LoginSuccessCallbackHandler onSuccessCallback;

        protected PlayFabLoginRequest(LoginSuccessCallbackHandler onSuccessCallback, EndpointHandler endpoint) : this(onSuccessCallback, null, endpoint) { }

        protected PlayFabLoginRequest(LoginSuccessCallbackHandler onSuccessCallback, FailureCallbackHandler onFailureCallback, EndpointHandler endpoint) : base(onFailureCallback, endpoint)
        {
            this.onSuccessCallback = onSuccessCallback;
        }

        protected override void OnSuccess(LoginResult result)
        {
            Assert.IsNotNull(result, "PlayFab returned null login result.");

            if (onSuccessCallback != null)
            {
                onSuccessCallback(result.PlayFabId, result.NewlyCreated);
            }
        }
    }
}
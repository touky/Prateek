namespace Mayfair.Core.Code.Backend.ServiceProviders.PlayFab.Requests
{
    using System.Collections.Generic;
    using global::PlayFab.ClientModels;

    public class LoginWithCustomIdRequest : PlayFabLoginRequest
    {
        protected override object CustomData
        {
            get => null;
        }

        protected override Dictionary<string, string> ExtraHeaders
        {
            get => null;
        }

        public LoginWithCustomIdRequest(string playerId, LoginSuccessCallbackHandler onSuccessCallback, FailureCallbackHandler onFailureCallback, EndpointHandler endpoint)
            : base(onSuccessCallback, onFailureCallback, endpoint)
        {
            infoRequestParams.GetUserAccountInfo = true;

            request = new LoginWithCustomIDRequest
            {
                CustomId = playerId,
                CreateAccount = true
            };
        }
    }
}
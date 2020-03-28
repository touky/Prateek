namespace Mayfair.Core.Code.Backend.ServiceProviders.PlayFab.Requests
{
    using System;
    using System.Collections.Generic;
    using global::PlayFab;
    using global::PlayFab.ClientModels;
    using global::PlayFab.SharedModels;
    using Utils.Debug;

    public abstract class BasePlayFabRequest<TRequest, TResult> : IPlayFabRequest where TRequest : PlayFabRequestCommon where TResult : PlayFabResultCommon
    {
        public delegate void EndpointHandler(TRequest request, Action<TResult> successCallback, Action<PlayFabError> errorCallback, object customData, Dictionary<string, string> customHeaders);
        
        protected TRequest request;
        protected GetPlayerCombinedInfoRequestParams infoRequestParams;
        protected Guid requestGuid;

        private EndpointHandler endpoint;
        private FailureCallbackHandler onFailureCallback;

        protected abstract object CustomData { get; }
        protected abstract Dictionary<string, string> ExtraHeaders { get; }

        protected BasePlayFabRequest(EndpointHandler endpoint) : this(null, endpoint) { }

        protected BasePlayFabRequest(FailureCallbackHandler onFailureCallback, EndpointHandler endpoint)
        {
            this.endpoint = endpoint;
            this.onFailureCallback = onFailureCallback;
            infoRequestParams = new GetPlayerCombinedInfoRequestParams();
            requestGuid = Guid.NewGuid();
        }

        public void Dispatch()
        {
            DebugTools.Log($"Dispatching {GetType().Name} with ID {requestGuid}", DebugTools.LogLevel.Verbose);

            if (endpoint != null)
            {
                endpoint(request, OnSuccess, OnFailure, CustomData, ExtraHeaders);
            }
            else
            {
                DebugTools.LogError("Trying to dispatch a request with no api endpoint set!  This request will be ignored.\nRequest: {0}", request.GetType().ToString());
            }
        }

        protected abstract void OnSuccess(TResult result);

        protected virtual void OnFailure(PlayFabError error)
        {
            DebugTools.LogWarning($"{GetType().Name} with ID {requestGuid} failed!", DebugTools.LogLevel.Verbose);

            if (onFailureCallback != null)
            {
                onFailureCallback();
            }
        }
    }
}
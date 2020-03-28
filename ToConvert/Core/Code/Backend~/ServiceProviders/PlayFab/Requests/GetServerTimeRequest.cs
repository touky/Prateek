namespace Mayfair.Core.Code.Backend.ServiceProviders.PlayFab.Requests
{
    using System;
    using global::PlayFab;
    using global::PlayFab.ClientModels;
    using Types.Extensions;
    using Utils.Debug;

    /// <remarks>
    ///     This is not typical for PlayFab requests and should not be used as an example.
    ///     See <see cref="BasePlayFabRequest{TRequest,TResult}" /> instead.
    /// </remarks>
    public class GetServerTimeRequest
    {
        private Action<DateTime> onSuccess;

        public void Dispatch(Action<DateTime> onSuccess)
        {
            this.onSuccess = onSuccess;

            GetTimeRequest timeRequest = new GetTimeRequest();
            PlayFabClientAPI.GetTime(timeRequest, GetTimeResultCallback, GetTimeErrorCallback);
        }

        private void GetTimeResultCallback(GetTimeResult result)
        {
            DebugTools.Log($"Successfully retrieved server time: {result.Time.ToString("G")}", DebugTools.LogLevel.Verbose);
            onSuccess.SafeInvoke(result.Time);
        }

        private void GetTimeErrorCallback(PlayFabError error)
        {
            DebugTools.LogWarning($"Failed to retrieve server time. Error Details: {error.ErrorMessage}");
        }
    }
}
namespace Mayfair.Core.Code.Backend.ServiceProviders
{
    public delegate void FailureCallbackHandler();
    public delegate void LoginSuccessCallbackHandler(string playerId, bool isNewPlayer);
}
namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Service;

    public abstract class ResourceDependentServiceProvider<TService, TProvider> : Service.ServiceProviderBehaviour
        where TService : ServiceCommunicatorBehaviour<TService, TProvider>
        where TProvider : ResourceDependentServiceProvider<TService, TProvider>
    {
        #region Properties
        public abstract string[] ResourceKeywords { get; }
        #endregion

        #region Class Methods
        public abstract RequestCallbackOnChange GetResourceChangeRequest(ILightMessageCommunicator communicator);
        public abstract void OnResourceChanged(TService service, ResourcesHaveChangedResponse message);
        #endregion
    }
}

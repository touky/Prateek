namespace Assets.Prateek.ToConvert.Resources
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Resources.Messages;
    using Assets.Prateek.ToConvert.Service;

    public abstract class ResourceDependentServiceProvider<TService, TProvider> : ServiceProviderBehaviour
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

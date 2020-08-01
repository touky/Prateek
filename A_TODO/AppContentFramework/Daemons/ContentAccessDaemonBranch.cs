namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Resources.Messages;
    using Prateek.DaemonFramework.Code.Servants;
    using Prateek.CommandFramework.Tools;
    using Prateek.CommandFramework.TransmitterReceiver;

    public abstract class ContentAccessServant<TDaemon, TServant>
        : ServantTickableBehaviour<TDaemon, TServant>
        where TDaemon : CommandReceiverDaemon<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Properties
        public abstract string[] ResourceKeywords { get; }
        #endregion

        #region Class Methods
        public abstract RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter);
        public abstract void OnResourceChanged(TDaemon service, ResourcesHaveChangedResponse notice);
        #endregion
    }
}

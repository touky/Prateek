namespace Prateek.A_TODO.Runtime.AppContentFramework.Daemons
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Messages;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Tools;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class ContentAccessServant<TDaemon, TServant>
        : Servant<TDaemon, TServant>
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
        where TServant : ContentAccessServant<TDaemon, TServant>
    {
        #region Properties
        public abstract string[] ResourceKeywords { get; }
        #endregion

        #region Class Methods
        //todo public abstract RequestAccessToContent GetResourceChangeRequest(ICommandEmitter transmitter);
        public abstract void OnResourceChanged(TDaemon service, ContentAccessChangedResponse notice);
        #endregion
    }
}

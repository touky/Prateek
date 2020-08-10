namespace Prateek.Runtime.CommandFramework
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class ReceiverDaemon<TDaemon>
        : Daemon<TDaemon>
        , ICommandReceiverOwner
        , IEarlyUpdateTickable
        where TDaemon : ReceiverDaemon<TDaemon>
    {
        #region ICommandReceiverOwner Members
        public abstract void DefineReceptionActions(ICommandReceiver receiver);
        #endregion

        #region IEarlyUpdateTickable Members
        public virtual void EarlyUpdate()
        {
            this.Get<ICommandReceiver>().ProcessReceivedCommands();
        }
        #endregion
    }
}

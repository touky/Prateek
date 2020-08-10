namespace Prateek.Runtime.CommandFramework
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class ReceiverDaemonOverseer<TDaemon, TServant>
        : DaemonOverseer<TDaemon, TServant>
        , ICommandReceiverOwner
        , IEarlyUpdateTickable
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
        where TServant : class, IServant
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

namespace Prateek.Runtime.CommandFramework
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.TickableFramework.Interfaces;

    public abstract class ReceiverDaemonOverseer<TDaemon, TServant>
        : DaemonOverseer<TDaemon, TServant>
        , ICommandReceiverOwner
        , IEarlyUpdateTickable
        where TDaemon : ReceiverDaemonOverseer<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Fields
        private ICommandReceiver commandReceiver;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            this.InitializeReceiver(ref commandReceiver);

            base.Awake();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            commandReceiver.Kill();
        }
        #endregion

        #region ICommandReceiverOwner Members
        public ICommandReceiver CommandReceiver { get { return commandReceiver; } }

        public string Name { get { return name; } }

        public abstract void DefineCommandReceiverActions();
        #endregion

        #region IEarlyUpdateTickable Members
        public virtual void EarlyUpdate()
        {
            commandReceiver.ProcessReceivedCommands();
        }
        #endregion
    }
}

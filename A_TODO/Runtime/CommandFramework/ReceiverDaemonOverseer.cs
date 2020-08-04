namespace Prateek.A_TODO.Runtime.CommandFramework.Tools
{
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DaemonFramework.Interfaces;

    public abstract class ReceiverDaemonOverseer<TDaemon, TServant>
        : DaemonOverseer<TDaemon, TServant>, ICommandReceiverOwner
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

        protected virtual void OnDestroy()
        {
            commandReceiver.Kill();
        }

        protected virtual void Update()
        {
            commandReceiver.ProcessReceivedCommands();
        }
        #endregion

        #region ICommandReceiverOwner Members
        public ICommandReceiver CommandReceiver
        {
            get { return commandReceiver; }
        }

        public string Name
        {
            get { return name; }
        }

        public abstract void DefineCommandReceiverActions();
        #endregion
    }
}

namespace Prateek.A_TODO.Runtime.CommandFramework.Tools
{
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.DaemonFramework;

    public abstract class ReceiverDaemon<TDaemon>
        : Daemon<TDaemon>, ICommandReceiverOwner
        where TDaemon : ReceiverDaemon<TDaemon>
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

        protected virtual void Update()
        {
            commandReceiver.ProcessReceivedCommands();
        }

        protected virtual void OnDestroy()
        {
            commandReceiver.Kill();
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

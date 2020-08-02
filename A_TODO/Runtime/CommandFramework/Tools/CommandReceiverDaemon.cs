namespace Prateek.A_TODO.Runtime.CommandFramework.Tools
{
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using UnityEngine;

    public abstract class CommandReceiverDaemon<TDaemon, TServant>
        : DaemonOverseer<TDaemon, TServant>, ICommandReceiverOwner
        where TDaemon : CommandReceiverDaemon<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Fields
        private ICommandReceiver commandReceiver;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            InitNoticeReceiver();

            base.Awake();
        }

        protected virtual void Update()
        {
            UpdateNoticeReceiver();
        }

        protected virtual void OnDestroy()
        {
            this.commandReceiver.CleanUp();
        }
        #endregion

        #region Class Methods
        protected void InitNoticeReceiver()
        {
            this.commandReceiver = CommandDaemon.CreateCommandReceiver(this);
            SetupCommandReceiverCallback();
            this.commandReceiver.ApplyCallbacks();
        }

        protected void UpdateNoticeReceiver()
        {
            CommandReceiver.ProcessAllCommands();
        }

        protected abstract void SetupCommandReceiverCallback();
        #endregion

        #region IMessageCommunicatorOwner Members
        public ICommandReceiver CommandReceiver
        {
            get { return this.commandReceiver; }
        }

        public string Name
        {
            get { return name; }
        }

        public Transform Transform
        {
            get { return transform; }
        }

        public virtual void CommandReceived() { }
        #endregion
    }
}

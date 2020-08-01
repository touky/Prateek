namespace Mayfair.Core.Code.BaseBehaviour
{
    using Prateek.CommandFramework;
    using Prateek.CommandFramework.TransmitterReceiver;
    using UnityEngine;

    public abstract class CommandReceiverOwner : MonoBehaviour, ICommandReceiverOwner
    {
        #region Fields
        private ICommandReceiver commandReceiver;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            InitCommandReceiver();
        }

        protected virtual void Update()
        {
            UpdateCommandReceiver();
        }

        protected virtual void OnDestroy()
        {
            this.commandReceiver.CleanUp();
        }
        #endregion

        #region Class Methods
        protected void InitCommandReceiver()
        {
            this.commandReceiver = CommandDaemon.CreateCommandReceiver(this);
            SetupCommandReceiverCallback();
            this.commandReceiver.ApplyCallbacks();
        }

        protected void UpdateCommandReceiver()
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
        #endregion
    }
}

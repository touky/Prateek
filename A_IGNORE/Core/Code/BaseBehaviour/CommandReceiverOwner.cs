namespace Mayfair.Core.Code.BaseBehaviour
{
    using Prateek.A_TODO.Runtime.CommandFramework;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using UnityEngine;

    public abstract class CommandReceiverOwner : MonoBehaviour, ICommandReceiverOwner
    {
        #region Fields
        private ICommandReceiver commandReceiver;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            this.InitializeReceiver(ref commandReceiver);
        }

        protected virtual void Update()
        {
            commandReceiver.ProcessReceivedCommands();
        }

        protected virtual void OnDestroy()
        {
            this.commandReceiver.Kill();
        }
        #endregion

        #region Class Methods
        public abstract void DefineCommandReceiverActions();
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

namespace Mayfair.Core.Code.BaseBehaviour
{
    using Mayfair.Core.Code.Messaging;
    using Mayfair.Core.Code.Messaging.Communicator;
    using UnityEngine;

    public abstract class CommunicatorBehaviour : MonoBehaviour, IMessageCommunicatorOwner
    {
        #region Fields
        private IMessageCommunicator communicator;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            InitCommunicator();
        }

        protected virtual void Update()
        {
            UpdateCommunicator();
        }

        protected virtual void OnDestroy()
        {
            this.communicator.CleanUp();
        }
        #endregion

        #region Class Methods
        protected void InitCommunicator()
        {
            this.communicator = MessageService.CreateNewCommunicator(this);
            SetupCommunicatorCallback();
            this.communicator.ApplyCallbacks();
        }

        protected void UpdateCommunicator()
        {
            if (Communicator.HasMessage())
            {
                Communicator.ProcessAllMessages();
            }
        }

        protected abstract void SetupCommunicatorCallback();
        #endregion

        #region IMessageCommunicatorOwner Members
        public IMessageCommunicator Communicator
        {
            get { return this.communicator; }
        }

        public string Name
        {
            get { return name; }
        }

        public Transform Transform
        {
            get { return transform; }
        }

        public virtual void MessageReceived() { }
        #endregion
    }
}

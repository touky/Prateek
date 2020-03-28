namespace Mayfair.Core.Code.BaseBehaviour
{
    using Mayfair.Core.Code.Messaging;
    using Mayfair.Core.Code.Messaging.Communicator;
    using UnityEngine;

    public abstract class AutoDisableCommunicatorBehaviour : AutoDisableBehaviour, IMessageCommunicatorOwner
    {
        #region Fields
        private IMessageCommunicator communicator;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            InitCommunicator();
        }

        protected void Update()
        {
            UpdateCommunicator();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

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

        public void MessageReceived()
        {
            WakeUp();
        }
        #endregion
    }
}

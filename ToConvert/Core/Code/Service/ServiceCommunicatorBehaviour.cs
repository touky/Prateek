namespace Mayfair.Core.Code.Service
{
    using Mayfair.Core.Code.Messaging;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Service.Interfaces;
    using UnityEngine;

    public abstract class ServiceCommunicatorBehaviour<TService, TProvider> : ServiceSingletonBehaviour<TService, TProvider>, IMessageCommunicatorOwner
        where TService : ServiceCommunicatorBehaviour<TService, TProvider>
        where TProvider : IServiceProvider
    {
        #region Fields
        private IMessageCommunicator communicator;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            InitCommunicator();

            base.Awake();
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

        public abstract void MessageReceived();
        #endregion
    }
}

namespace Assets.Prateek.ToConvert.Service
{
    using Assets.Prateek.ToConvert.Messaging;
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Service.Interfaces;
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

        protected virtual void OnDestroy()
        {
            communicator.CleanUp();
        }

        protected virtual void Update()
        {
            UpdateCommunicator();
        }
        #endregion

        #region Messaging
        protected abstract void SetupCommunicatorCallback();
        #endregion

        #region Class Methods
        protected void InitCommunicator()
        {
            communicator = MessageService.CreateNewCommunicator(this);
            SetupCommunicatorCallback();
            communicator.ApplyCallbacks();
        }

        protected void UpdateCommunicator()
        {
            if (Communicator.HasMessage())
            {
                Communicator.ProcessAllMessages();
            }
        }
        #endregion

        #region IMessageCommunicatorOwner Members
        public IMessageCommunicator Communicator
        {
            get { return communicator; }
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

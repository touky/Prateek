namespace Assets.Prateek.ToConvert.BaseBehaviour
{
    using Assets.Prateek.ToConvert.Messaging;
    using Assets.Prateek.ToConvert.Messaging.Communicator;
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

        protected override void OnDestroy()
        {
            base.OnDestroy();

            communicator.CleanUp();
        }

        protected void Update()
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

        public void MessageReceived()
        {
            WakeUp();
        }
        #endregion
    }
}

namespace Assets.Prateek.ToConvert.Messaging.Tools
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using UnityEngine;

    public abstract class RegulatedMessageSender<TMessage, TCommunicator>
        where TMessage : Message, new()
        where TCommunicator : ILightMessageCommunicator
    {
        #region Static and Constants
        private const float DEFAULT_COOLDOWN = 0.1f;
        #endregion

        #region Fields
        private double cooldown;
        private double timeMark;
        protected TMessage nextMessage;
        protected TCommunicator communicator;
        #endregion

        #region Properties
        public TMessage NextMessage
        {
            get { return nextMessage; }
        }
        #endregion

        #region Constructors
        protected RegulatedMessageSender(TCommunicator communicator, double cooldown = DEFAULT_COOLDOWN)
        {
            this.cooldown = cooldown;
            timeMark = Time.realtimeSinceStartup;
            this.communicator = communicator;

            MarkTime();
        }
        #endregion

        #region Class Methods
        public void Create()
        {
            if (nextMessage == null)
            {
                nextMessage = Message.Create<TMessage>();
            }
        }

        public bool TrySend()
        {
            if (Time.realtimeSinceStartup - timeMark < cooldown)
            {
                return false;
            }

            if (nextMessage == null)
            {
                nextMessage = Message.Create<TMessage>();
            }

            DoSend();

            nextMessage = null;
            MarkTime();

            return true;
        }

        private void MarkTime()
        {
            timeMark = Time.realtimeSinceStartup;
        }

        protected abstract void DoSend();
        #endregion
    }
}

namespace Prateek.CommandFramework.Tools
{
    using Commands.Core;
    using Prateek.CommandFramework.TransmitterReceiver;
    using UnityEngine;

    public abstract class RegulatedNoticeSender<TNotice, TNoticeReceiver>
        where TNotice : Command, new()
        where TNoticeReceiver : ICommandEmitter
    {
        private const float DEFAULT_COOLDOWN = 0.1f;

        #region Fields
        private double cooldown;
        private double timeMark;
        protected TNotice nextMessage;
        protected TNoticeReceiver transmitter;
        #endregion

        #region Properties
        public TNotice NextMessage
        {
            get { return nextMessage; }
        }
        #endregion

        #region Constructors
        protected RegulatedNoticeSender(TNoticeReceiver transmitter, double cooldown = DEFAULT_COOLDOWN)
        {
            this.cooldown = cooldown;
            timeMark = Time.realtimeSinceStartup;
            this.transmitter = transmitter;

            MarkTime();
        }
        #endregion

        #region Class Methods
        public void Create()
        {
            if (nextMessage == null)
            {
                nextMessage = Command.Create<TNotice>();
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
                nextMessage = Command.Create<TNotice>();
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

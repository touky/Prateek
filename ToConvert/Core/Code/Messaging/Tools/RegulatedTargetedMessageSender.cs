namespace Mayfair.Core.Code.Messaging.Tools
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;

    public class RegulatedTargetedMessageSender<TMessage> : RegulatedMessageSender<TMessage, IMessageCommunicator>
        where TMessage : TargetedMessage, new()
    {
        #region Constructors
        public RegulatedTargetedMessageSender(IMessageCommunicator communicator) : base(communicator) { }
        public RegulatedTargetedMessageSender(IMessageCommunicator communicator, double cooldown) : base(communicator, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            communicator.Send(nextMessage);
        }
        #endregion
    }
}

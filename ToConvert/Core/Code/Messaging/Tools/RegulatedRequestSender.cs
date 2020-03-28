namespace Mayfair.Core.Code.Messaging.Tools
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;

    public class RegulatedRequestSender<TMessage> : RegulatedMessageSender<TMessage, IMessageCommunicator>
        where TMessage : DirectMessage, new()
    {
        #region Constructors
        public RegulatedRequestSender(IMessageCommunicator communicator) : base(communicator) { }
        public RegulatedRequestSender(IMessageCommunicator communicator, double cooldown) : base(communicator, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            communicator.Send(nextMessage);
        }
        #endregion
    }
}

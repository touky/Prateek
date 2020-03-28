namespace Mayfair.Core.Code.Messaging.Tools
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;

    public class RegulatedDirectMessageSender<TMessage> : RegulatedMessageSender<TMessage, ILightMessageCommunicator>
        where TMessage : DirectMessage, new()
    {
        #region Constructors
        public RegulatedDirectMessageSender(ILightMessageCommunicator communicator) : base(communicator) { }
        public RegulatedDirectMessageSender(ILightMessageCommunicator communicator, double cooldown) : base(communicator, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            communicator.Send(nextMessage);
        }
        #endregion
    }
}

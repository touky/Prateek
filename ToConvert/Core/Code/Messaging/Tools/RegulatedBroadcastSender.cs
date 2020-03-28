namespace Mayfair.Core.Code.Messaging.Tools
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;

    public class RegulatedBroadcastSender<TMessage> : RegulatedMessageSender<TMessage, ILightMessageCommunicator>
        where TMessage : BroadcastMessage, new()
    {
        #region Constructors
        public RegulatedBroadcastSender(ILightMessageCommunicator communicator) : base(communicator) { }
        public RegulatedBroadcastSender(ILightMessageCommunicator communicator, double cooldown) : base(communicator, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            communicator.Broadcast(nextMessage);
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.Messaging.Tools
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;

    public class RegulatedResponseSender<TMessage> : RegulatedMessageSender<TMessage, ILightMessageCommunicator>
        where TMessage : DirectMessage, new()
    {
        #region Constructors
        public RegulatedResponseSender(ILightMessageCommunicator communicator) : base(communicator) { }
        public RegulatedResponseSender(ILightMessageCommunicator communicator, double cooldown) : base(communicator, cooldown) { }
        #endregion

        #region Class Methods
        protected override void DoSend()
        {
            communicator.Send(nextMessage);
        }
        #endregion
    }
}
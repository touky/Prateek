namespace Assets.Prateek.ToConvert.Messaging.Tools
{
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;

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

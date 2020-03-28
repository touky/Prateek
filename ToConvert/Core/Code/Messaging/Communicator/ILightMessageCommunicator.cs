namespace Mayfair.Core.Code.Messaging.Communicator
{
    using Mayfair.Core.Code.Messaging.Messages;

    public interface ILightMessageCommunicator
    {
        #region Properties
        IMessageCommunicatorOwner Owner { get; }
        #endregion

        #region Class Methods
        //Sending
        void Broadcast(BroadcastMessage message);
        void Send(DirectMessage message);
        void Send(ResponseMessage message);
        #endregion
    }
}

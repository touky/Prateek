namespace Mayfair.Core.Code.Messaging.Communicator
{
    using Mayfair.Core.Code.Messaging.Messages;

    public interface IMessageCommunicator : ILightMessageCommunicator
    {
        #region Class Methods
        //Sending
        void Send(TargetedMessage message);
        void CleanUp();
        //Retrieving
        bool HasMessage();
        void ProcessAllMessages();
        #endregion

        #region Callbacks
        void AddCallback<T>(MessageCallback<T> messageCallback) where T : Message;
        void RemoveCallback<T>() where T : Message;
        void ClearCallbacks();
        void ApplyCallbacks();
        #endregion
    }
}


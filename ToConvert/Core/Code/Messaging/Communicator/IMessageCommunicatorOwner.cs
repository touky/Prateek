namespace Mayfair.Core.Code.Messaging.Communicator
{
    using UnityEngine;

    public interface IMessageCommunicatorOwner
    {
        #region Properties
        IMessageCommunicator Communicator { get; }
        string Name { get; }
        Transform Transform { get; }
        #endregion

        #region Class Methods
        void MessageReceived();
        #endregion
    }
}

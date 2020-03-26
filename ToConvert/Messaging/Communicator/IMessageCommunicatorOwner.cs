namespace Assets.Prateek.ToConvert.Messaging.Communicator
{
    using UnityEngine;

    public interface IMessageCommunicatorOwner
    {
        #region Properties
        IMessageCommunicator Communicator { get; }
        string Name { get; }
        Transform Transform { get; }
        #endregion

        #region Messaging
        void MessageReceived();
        #endregion
    }
}

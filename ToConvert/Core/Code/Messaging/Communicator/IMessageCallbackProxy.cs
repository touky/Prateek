namespace Mayfair.Core.Code.Messaging.Communicator
{
    using Mayfair.Core.Code.Messaging.Messages;

    public interface IMessageCallbackProxy
    {
        #region Class Methods
        void Invoke(Message message);
        #endregion
    }
}

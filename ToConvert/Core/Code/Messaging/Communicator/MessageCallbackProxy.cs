namespace Mayfair.Core.Code.Messaging.Communicator
{
    using System.Diagnostics;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Utils.Debug;
    using UnityEngine.Assertions;

    public delegate void MessageCallback<T>(T message) where T : Message;

    [DebuggerDisplay("Callback: {callback.Method.Name}, Type: {GetType().Name}")]
    public class MessageCallbackProxy<T> : IMessageCallbackProxy
        where T : Message
    {
        #region Fields
        private MessageCallback<T> callback;
        #endregion

        #region Constructors
        public MessageCallbackProxy(MessageCallback<T> callback)
        {
            this.callback = callback;
        }
        #endregion

        #region IMessageCallbackProxy Members
        public void Invoke(Message message)
        {
            T typedMessage = message as T;
            Assert.IsNotNull(typedMessage, $"typedMessage is null for: message = {message.GetType().Name}, T = {typeof(T).Name}");
            if (typedMessage != null && this.callback != null)
            {
                this.callback.Invoke(typedMessage);
            }
        }
        #endregion
    }
}

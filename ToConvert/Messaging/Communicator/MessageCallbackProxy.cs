namespace Assets.Prateek.ToConvert.Messaging.Communicator
{
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.Messaging.Messages;
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
            var typedMessage = message as T;
            Assert.IsNotNull(typedMessage, $"typedMessage is null for: message = {message.GetType().Name}, T = {typeof(T).Name}");
            if (typedMessage != null && callback != null)
            {
                callback.Invoke(typedMessage);
            }
        }
        #endregion
    }
}

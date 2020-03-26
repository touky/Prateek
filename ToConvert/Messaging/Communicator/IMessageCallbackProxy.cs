namespace Assets.Prateek.ToConvert.Messaging.Communicator
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public interface IMessageCallbackProxy
    {
        #region Class Methods
        void Invoke(Message message);
        #endregion
    }
}

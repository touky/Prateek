namespace Prateek.CommandFramework.TransmitterReceiver
{
    using Commands.Core;

    public interface ICommandEmitter
    {
        #region Properties
        ICommandReceiverOwner Owner { get; }
        #endregion

        #region Class Methods
        //Sending
        void Broadcast(BroadcastCommand command);
        void Send(DirectCommand command);
        void Send(ResponseCommand command);
        #endregion
    }
}

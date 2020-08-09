namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public interface ICommandEmitter
    {
        #region Properties
        ICommandReceiverOwner Owner { get; }
        #endregion

        #region Class Methods
        //Sending
        void Send(BroadcastCommand command);
        void Send(DirectCommand command);
        void Send(ResponseCommand command);
        #endregion
    }
}

namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface ICommandEmitter : IGadget
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

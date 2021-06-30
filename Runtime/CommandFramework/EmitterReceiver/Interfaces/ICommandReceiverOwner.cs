namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public interface ICommandReceiverOwner
        : IGadgetOwner
    {
        #region Class Methods
        ICommandReceiver Receiver { get; }
        void DefineReceptionActions(ICommandReceiver receiver);
        #endregion
    }
}

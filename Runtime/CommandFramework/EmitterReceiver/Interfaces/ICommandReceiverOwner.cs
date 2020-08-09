namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    public interface ICommandReceiverOwner
    {
        #region Properties
        ICommandReceiver CommandReceiver { get; }
        string Name { get; }
        #endregion

        #region Class Methods
        void DefineCommandReceiverActions();
        #endregion
    }
}

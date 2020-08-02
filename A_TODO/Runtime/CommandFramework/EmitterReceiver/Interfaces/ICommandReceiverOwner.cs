namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    public interface ICommandReceiverOwner
    {
        #region Properties
        ICommandReceiver CommandReceiver { get; }
        string Name { get; }
        #endregion
    }
}

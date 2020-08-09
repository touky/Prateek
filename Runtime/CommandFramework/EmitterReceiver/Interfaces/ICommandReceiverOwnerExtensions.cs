namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    public static class ICommandReceiverOwnerExtensions
    {
        #region Class Methods
        public static void InitializeReceiver(this ICommandReceiverOwner owner, ref ICommandReceiver receiver)
        {
            receiver = CommandDaemon.CreateCommandReceiver(owner);
            owner.DefineCommandReceiverActions();
            receiver.ApplyActionChanges();
        }
        #endregion
    }
}

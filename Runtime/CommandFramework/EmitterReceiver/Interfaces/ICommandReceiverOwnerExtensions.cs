namespace Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public static class ICommandReceiverOwnerExtensions
    {
        public static void InitializeReceiver(this ICommandReceiverOwner owner, ref ICommandReceiver receiver)
        {
            receiver = CommandDaemon.CreateCommandReceiver(owner);
            owner.DefineCommandReceiverActions();
            receiver.ApplyActionChanges();
        }
    }
}

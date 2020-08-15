namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using JetBrains.Annotations;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    [UsedImplicitly]
    public class CommandReceiverInstantiator
        : IGadgetInstantiator
    {
        #region IGadgetInstantiator Members
        public int DefaultPriority
        {
            get { return typeof(CommandReceiverInstantiator).GetHashCode(); }
        }

        public void Create(IGadgetOwner owner)
        {
            if (!(owner is ICommandReceiverOwner receiverOwner))
            {
                return;
            }

            var receiver = new CommandReceiver(receiverOwner);
            {
                owner.GadgetPouch.Add<ICommandEmitter>(receiver);
                owner.GadgetPouch.Add<ICommandReceiver>(receiver);
                owner.GadgetPouch.Add(receiver);
            }
            receiverOwner.DefineReceptionActions(receiver);
            receiver.ApplyActionChanges();
        }
        #endregion
    }
}

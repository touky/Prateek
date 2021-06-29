namespace Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces
{
    using JetBrains.Annotations;
    using Prateek.Runtime.GadgetFramework;
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
        
        public void Declare(IInstantiatorBinder binder)
        {
            binder.BindTo<ICommandReceiverOwner>();
            binder.InjectGadgetTo<ICommandReceiver>();
            binder.AddGadgetAs<ICommandEmitter>();
            binder.AddGadgetAs<ICommandReceiver>();
        }

        public void Bind(IGadgetBinder binder)
        {
            binder.Bind(new CommandReceiver());
        }
        #endregion
    }
}

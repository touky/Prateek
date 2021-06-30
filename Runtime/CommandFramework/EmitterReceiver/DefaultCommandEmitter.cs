namespace Prateek.Runtime.CommandFramework.EmitterReceiver
{
    using System;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    [Obsolete("validate this")]
    internal class DefaultCommandEmitter
        : ICommandReceiverOwner
    {
        #region Constructors
        public DefaultCommandEmitter()
        {
            this.AutoRegister();
        }
        #endregion

        #region ICommandReceiverOwner Members
        private ICommandReceiver Receiver { get; set; }
        ICommandReceiver ICommandReceiverOwner.Receiver { get { return Receiver; } }
        public ICommandEmitter Emitter { get { return Receiver; } }
        public IGadgetPouch GadgetPouch { get; private set; }

        public string Name { get { return GetType().Name; } }

        public void DefineReceptionActions(ICommandReceiver receiver) { }
        #endregion
    }
}

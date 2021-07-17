namespace Prateek.Runtime.CommandFramework.Gadgets
{
    using System;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    [Obsolete("validate this")]
    internal class DefaultCommandEmitter
        : CommandTools.IReceiverOwner
    {
        #region Constructors
        public DefaultCommandEmitter()
        {
            this.AutoRegister();
        }
        #endregion

        #region IReceiverOwner Members
        private CommandTools.IReceiver Receiver { get; set; }
        CommandTools.IReceiver CommandTools.IReceiverOwner.Receiver { get { return Receiver; } }
        public CommandTools.IEmitter Emitter { get { return Receiver; } }
        public IGadgetPouch GadgetPouch { get; private set; }

        public string Name { get { return GetType().Name; } }

        public void DefineReceptionActions(CommandTools.IReceiver receiver) { }
        #endregion
    }
}

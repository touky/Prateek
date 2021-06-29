namespace Prateek.Runtime.CommandFramework.EmitterReceiver
{
    using System;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.GadgetFramework;

    [Obsolete("validate this")]
    internal class DefaultCommandEmitter
        : ICommandReceiverOwner
    {
        #region Fields
        private GadgetPouch gadgetPouch = new GadgetPouch();
        private ICommandEmitter emitter = null;
        #endregion

        #region Properties
        public ICommandEmitter Emitter { get { return emitter; } }
        #endregion

        #region Constructors
        public DefaultCommandEmitter()
        {
            this.AutoRegister();

            emitter = this.Get<ICommandEmitter>();
        }
        #endregion

        #region ICommandReceiverOwner Members
        public string Name { get { return GetType().Name; } }

        public IGadgetPouch GadgetPouch { get { return gadgetPouch; } }

        public void DefineReceptionActions(ICommandReceiver receiver) { }
        #endregion
    }
}

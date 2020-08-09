namespace Prateek.Runtime.CommandFramework.EmitterReceiver
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    internal class DefaultCommandEmitter : ICommandReceiverOwner
    {
        #region Fields
        private ICommandReceiver commandReceiver;
        #endregion

        #region Constructors
        public DefaultCommandEmitter()
        {
            this.InitializeReceiver(ref commandReceiver);
        }
        #endregion

        #region ICommandReceiverOwner Members
        public ICommandReceiver CommandReceiver { get { return commandReceiver; } }

        public string Name { get { return GetType().Name; } }

        public void DefineCommandReceiverActions() { }
        #endregion
    }
}

namespace Prateek.A_TODO.Runtime.CommandFramework.Servants {
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using UnityEngine;

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

        #region IMessageCommunicatorOwner Members
        public ICommandReceiver CommandReceiver
        {
            get { return commandReceiver; }
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public void DefineCommandReceiverActions()
        {
        }
        #endregion
    }
}
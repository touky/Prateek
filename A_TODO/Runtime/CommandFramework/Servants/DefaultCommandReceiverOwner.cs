namespace Prateek.A_TODO.Runtime.CommandFramework.Servants {
    using System;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using UnityEngine;

    internal class DefaultCommandReceiverOwner : ICommandReceiverOwner
    {
        #region Fields
        private ICommandReceiver commandReceiver;
        #endregion

        #region Constructors
        public DefaultCommandReceiverOwner()
        {
            commandReceiver = CommandDaemon.CreateCommandReceiver(this);
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

        public Transform Transform
        {
            get { return null; }
        }

        public void CommandReceived()
        {
            throw new NotImplementedException($"Cannot receiver notices through the {typeof(DefaultCommandReceiverOwner).Name}");
        }
        #endregion
    }
}
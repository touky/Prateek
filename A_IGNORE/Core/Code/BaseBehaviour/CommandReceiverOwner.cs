namespace Mayfair.Core.Code.BaseBehaviour
{
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.AutoRegistration;
    using Prateek.Runtime.GadgetFramework;
    using UnityEngine;

    public abstract class CommandReceiverOwner
        : MonoBehaviour
        , ICommandReceiverOwner
    {
        #region Fields
        private GadgetPouch gadgetPouch = new GadgetPouch();
        #endregion

        #region Constructors
        protected CommandReceiverOwner()
        {
            this.AutoRegister();
        }
        #endregion

        #region Unity Methods
        protected virtual void Update()
        {
            this.Get<ICommandReceiver>().ProcessReceivedCommands();
        }
        #endregion

        #region ICommandReceiverOwner Members
        public string Name { get { return GetType().Name; } }

        public GadgetPouch GadgetPouch { get { return gadgetPouch; } }

        public virtual void DefineReceptionActions(ICommandReceiver receiver) { }
        #endregion
    }
}

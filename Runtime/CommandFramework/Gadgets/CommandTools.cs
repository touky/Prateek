namespace Prateek.Runtime.CommandFramework.Gadgets
{
    using System;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Commands.Core.Commands;
    using Prateek.Runtime.GadgetFramework.Interfaces;

    public abstract class CommandTools
        : GadgetTools
    {
        public static int InstantiatorHashCode()
        {
            return typeof(Instantiator).GetHashCode();
        }

        internal class Instantiator
            : IInstantiator
        {
            #region IGadgetInstantiator Members
            public int DefaultPriority
            {
                get { return typeof(Instantiator).GetHashCode(); }
            }
        
            public void Declare(IInstantiatorBinder binder)
            {
                binder.BindTo<IReceiverOwner>();
                binder.InjectGadgetTo<IReceiver>();
                binder.AddGadgetAs<IEmitter>();
                binder.AddGadgetAs<IReceiver>();
            }

            public void Bind(IGadgetBinder binder)
            {
                binder.Bind(new Receiver());
            }
            #endregion
        }

        public interface IEmitter
            : IGadget
        {
            #region Properties
            IReceiverOwner Owner { get; }
            #endregion

            #region Class Methods
            //Sending
            void Send(BroadcastCommand command);
            void Send(DirectCommand command);
            void Send(ResponseCommand command);
            #endregion
        }

        public interface IReceiver
            : IEmitter
        {
            #region Class Methods
            //Sending
            void Send(TargetedCommand command);

            /// <summary>
            ///     This needs to be called to flush and process all received commands
            /// </summary>
            void ProcessReceivedCommands();
            #endregion

            #region Setup
            void SetActionForReception(Action onCommandReceived);
            bool HasActionFor<T>()
                where T : Command;
            void SetActionFor<T>(CommandAction<T> action)
                where T : Command;
            void ClearActionFor<T>() where T : Command;
            void ClearAllActions();
            void ApplyActionChanges();
            #endregion
        }

        public interface IReceiverOwner
            : IOwner
        {
            #region Class Methods
            IReceiver Receiver { get; }
            void DefineReceptionActions(IReceiver receiver);
            #endregion
        }
    }
}
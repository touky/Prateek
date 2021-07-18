namespace Prateek.Runtime.StateMachineFramework
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using Prateek.Runtime.StateMachineFramework.DelegateStateMachines;

    public abstract class StateMachine
        : GadgetTools
    {
        private const string StateMachineProperty = "StateMachine";

        [UsedImplicitly]
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
                binder.BindGadgetTo<IOwner>();
                binder.InjectGadgetTo(StateMachineProperty);
                binder.AddGadgetAs<IMachine>();
            }

            public void Bind(IGadgetBinder binder)
            {
                binder.Bind(binder.GetInstance(StateMachineProperty));
            }
            #endregion
        }

        public new interface IOwner
            : GadgetTools.IOwner
        {
            IMachine CreateStateMachine();
            void Setup(IMachine stateMachine);
        }

        public interface IDelegateOwner<TStateMachine, TTrigger>
            : IOwner
            where TStateMachine : IStateMachine<MethodInfo, TTrigger>
            where TTrigger : struct, IConvertible
        {
            TStateMachine StateMachine { get; }
        }

        public interface IStepOwner
            : IOwner
        {
            DelegateStepMachine StateMachine { get; }
        }

        public interface IMachine
            : IGadget
        {
            void Reboot();
        }

        public interface IStateMachine<TState, TTrigger>
            : StateMachine.IMachine
        {
            #region Properties
            TState ActiveState { get; }
            TState IncomingState { get; }
            bool IsActive { get; }
            #endregion

            #region Class Methods
            void Reboot();
            void Step();
            void Trigger(TTrigger trigger);
            #endregion
        }
    }
}
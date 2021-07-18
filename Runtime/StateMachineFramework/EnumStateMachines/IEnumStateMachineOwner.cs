namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Reflection;
    using JetBrains.Annotations;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.GadgetFramework.Interfaces;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using UnityEngine.Assertions;

    public abstract class DelegateStateMachine
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
            , IStateMachineOwner
        {
            IMachine CreateStateMachine();
            void Setup(IMachine stateMachine);
        }

        public interface IOwner<TStateMachine, TTrigger>
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
    }

    public interface IEnumStateMachineOwner<TState>
        : IStateMachineOwner
        where TState : struct, IConvertible
    {
        #region Class Methods
        /// <summary>
        ///     Callback to inform the owner of a state change.
        /// </summary>
        /// <param name="endingState"></param>
        /// <param name="beginningState"></param>
        void ChangingState(TState endingState, TState beginningState);

        /// <summary>
        ///     Callback to inform the owner that this state is executing
        /// </summary>
        /// <param name="state"></param>
        void ExecutingState(TState state);
        #endregion
    }
}

namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    [Serializable]
    public abstract class EnumStateMachine<TState, TTrigger, TEnumComparer>
        : IStateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible
        where TEnumComparer : IEnumComparer<TState, TTrigger>, new()
    {
        #region Fields
        protected IEnumStateMachineOwner<TState> owner;
        protected TEnumComparer comparer;
        protected List<TState> states;
        protected int activeState;
        protected int incomingState;
        #endregion

        #region Constructors
        protected EnumStateMachine() { }

        protected EnumStateMachine(IEnumStateMachineOwner<TState> owner) : this()
        {
            if (states != null)
            {
                return;
            }

            var values     = Enum.GetValues(typeof(TState));
            var initStates = new TState[values.Length];
            values.CopyTo(initStates, 0);

            var sortedStates = new List<TState>(initStates);
            sortedStates.Sort((a, b) => { return Convert.ToInt32(a) - Convert.ToInt32(b); });

            Init(owner, sortedStates);
        }
        #endregion

        #region Class Methods
        protected void Init(IEnumStateMachineOwner<TState> owner, List<TState> initStates)
        {
            if (!typeof(TState).IsEnum)
            {
                throw new ArgumentException("TState must be an enum type");
            }

            this.owner = owner;
            states = initStates;
            comparer = new TEnumComparer();

            Reboot();
        }

        protected virtual void TryChangingState()
        {
            if (activeState != incomingState)
            {
                var endingState = ActiveState;

                activeState = incomingState;

                owner.ChangingState(endingState, ActiveState);
            }
        }

        protected int IndexOf(TState state)
        {
            return states.FindIndex(x =>
            {
                return comparer.Compare(x, state);
            });
        }
        #endregion

        #region IStateMachine<TState,TTrigger> Members
        public TState ActiveState
        {
            get { return states[activeState]; }
        }

        public TState IncomingState
        {
            get { return states[incomingState]; }
        }

        public abstract bool IsActive { get; }

        public void Step()
        {
            if (!IsActive)
            {
                return;
            }

            using (ProfilingScope.Open(ActiveState))
            {
                owner.ExecutingState(ActiveState);

                TryChangingState();
            }
        }

        public abstract void Trigger(TTrigger trigger);
        public abstract void Reboot();
        #endregion

        #region Nested type: ProfilingScope
        protected class ProfilingScope : StateMachineProfilingScope<TState, TTrigger>
        {
            #region Constructors
            protected ProfilingScope(TState state) : base(state) { }
            #endregion
        }
        #endregion
    }
}

namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Profiling;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using UnityEngine;

    [Serializable]
    public abstract class DelegateStateMachine<TTrigger, TEnumComparer>
        : IStateMachine<MethodInfo, TTrigger>
        , DelegateStateMachine.IMachine
        where TTrigger : struct, IConvertible
        where TEnumComparer : IEnumComparer<TTrigger>, new()
    {
        public delegate void StateDelegate(StateStatus stateStatus);

        #region Fields
        protected TEnumComparer comparer;
        protected List<StateInfo> states = new List<StateInfo>();
        protected int activeState;
        protected int incomingState;
        #endregion

        protected StateInfo CurrentState
        {
            get { return activeState < 0 ? default : states[activeState]; }
        }

        protected StateInfo NextState
        {
            get { return incomingState < 0 ? default : states[incomingState]; }
        }


        #region Constructors
        protected DelegateStateMachine() { }

        //protected DelegateStateMachine(DelegateStateMachine.IOwner owner) : this()
        //{
        //    if (states != null)
        //    {
        //        return;
        //    }

        //    var values = Enum.GetValues(typeof(TState));
        //    var initStates = new TState[values.Length];
        //    values.CopyTo(initStates, 0);

        //    var sortedStates = new List<TState>(initStates);
        //    sortedStates.Sort((a, b) => { return Convert.ToInt32(a) - Convert.ToInt32(b); });

        //    Init(owner, sortedStates);
        //}
        #endregion

        #region Class Methods
        protected void Init(List<StateDelegate> states)
        {
            foreach (var state in states)
            {
                Add(state);
            }

            comparer = new TEnumComparer();
        }

        protected void Add(StateDelegate state)
        {
            var index = IndexOf(state.Method);
            if (index == Const.INDEX_NONE)
            {
                states.Add(new StateInfo(state));
            }
        }

        protected virtual void TryChangingState()
        {
            if (activeState != incomingState)
            {
                var endingState = CurrentState;
                endingState.Run(StateStatus.End);

                activeState = incomingState;

                CurrentState.Run(StateStatus.Begin);
            }
        }

        protected int IndexOf(MethodInfo state)
        {
            return states.FindIndex(x => { return x.methodInfo == state; });
        }
        #endregion

        #region IStateMachine<TState,TTrigger> Members
        public DelegateStateMachine.IOwner Owner { get; private set; }

        public MethodInfo ActiveState
        {
            get { return activeState < 0 ? default : states[activeState].methodInfo; }
        }

        public MethodInfo IncomingState
        {
            get { return incomingState < 0 ? default : states[incomingState].methodInfo; }
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
                CurrentState.Run(StateStatus.Execute);

                TryChangingState();
            }
        }

        public abstract void Trigger(TTrigger trigger);
        public abstract void Reboot();
        #endregion

        #region IGadget
        public void Awake()
        {
            Owner.Setup(this);
            Reboot();
        }

        public void Kill()
        {
            activeState = -1;
        }
        #endregion

        #region Nested type: ProfilingScope
        protected class ProfilingScope : ProfilerScope
        {
            #region Constructors
            protected ProfilingScope(string sample)
                : base(sample)
            { }

            public static ProfilerScope Open(MethodInfo state)
            {
                return ProfilerScope.Open(state == null ? "INIT" : state.ToString());
            }
            #endregion
        }
        #endregion

        protected struct StateInfo
        {
            public MethodInfo methodInfo;
            public StateDelegate state;

            public StateInfo(StateDelegate state)
            {
                methodInfo = state.Method;
                this.state = state;
            }

            public void Run(StateStatus status)
            {
                if (methodInfo == null)
                {
                    return;
                }

                state(status);
            }
        }
    }
}
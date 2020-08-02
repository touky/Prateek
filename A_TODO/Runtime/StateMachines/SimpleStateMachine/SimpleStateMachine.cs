namespace Prateek.A_TODO.Runtime.StateMachines.SimpleStateMachine
{
    using System;
    using System.Collections.Generic;
    using Prateek.A_TODO.Runtime.StateMachines.Interfaces;

    [Serializable]
    public abstract class SimpleStateMachine<TState, TTrigger> : IStateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible
    {
        #region Fields
        protected ISimpleStateMachineOwner<TState, TTrigger> owner;
        protected int currentState;
        protected int nextState;
        protected List<TState> states;
#if ENABLE_PROFILER
        protected StateMachineDebug<TState, TTrigger> debug;
#endif
        #endregion

        #region Constructors
        /// <summary>
        ///     This constructor will use all states in the enum from the first to the last as default
        ///     You can customize the profiler logging format with profilerFormat
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateProfilerFormat">String.Format for the profiler logging</param>
        /// <param name="triggerProfilerFormat">String.Format for the profiler logging</param>
        protected SimpleStateMachine(ISimpleStateMachineOwner<TState, TTrigger> owner, string stateProfilerFormat = null, string triggerProfilerFormat = null)
        {
            if (this.states != null)
            {
                return;
            }

            Array values = Enum.GetValues(typeof(TState));
            TState[] initStates = new TState[values.Length];
            values.CopyTo(initStates, 0);

            List<TState> sortList = new List<TState>(initStates);
            sortList.Sort((a, b) => { return Convert.ToInt32(a) - Convert.ToInt32(b); });

            Setup(owner, stateProfilerFormat, triggerProfilerFormat, sortList);
        }

        /// <summary>
        ///     This constructor will use the list given in stateSequence as the sequence of states
        ///     You can create two state machines with the same enum and different orders with this CTor
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateSequence">List of the sequence of states</param>
        protected SimpleStateMachine(ISimpleStateMachineOwner<TState, TTrigger> owner, params TState[] stateSequence)
            : this(owner, null, null, stateSequence) { }

        /// <summary>
        ///     This constructor will use the list given in stateSequence as the sequence of states
        ///     You can create two state machines with the same enum and different orders with this CTor
        ///     You can customize the profiler logging format with profilerFormat
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateProfilerFormat">String.Format for the profiler logging</param>
        /// <param name="stateSequence">List of the sequence of states</param>
        protected SimpleStateMachine(ISimpleStateMachineOwner<TState, TTrigger> owner, string stateProfilerFormat, params TState[] stateSequence)
            : this(owner, stateProfilerFormat, null, stateSequence) { }

        /// <summary>
        ///     This constructor will use the list given in stateSequence as the sequence of states
        ///     You can create two state machines with the same enum and different orders with this CTor
        ///     You can customize the profiler logging format with profilerFormat
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateProfilerFormat">String.Format for the profiler logging</param>
        /// <param name="triggerProfilerFormat">String.Format for the profiler logging</param>
        /// <param name="stateSequence">List of the sequence of states</param>
        protected SimpleStateMachine(ISimpleStateMachineOwner<TState, TTrigger> owner, string stateProfilerFormat, string triggerProfilerFormat, params TState[] stateSequence)
        {
            Setup(owner, stateProfilerFormat, triggerProfilerFormat, stateSequence);
        }
        #endregion

        #region Class Methods
        protected virtual void GoToNextState()
        {
            if (this.currentState != this.nextState)
            {
                TState previousState = CurrentState;

                this.currentState = this.nextState;

                this.owner.OnStateChange(previousState, CurrentState);
            }
        }

        private void BeginFrame()
        {
#if ENABLE_PROFILER
            this.debug.ProfilerBeginFrame(CurrentState);
#endif
        }

        private void EndFrame()
        {
#if ENABLE_PROFILER
            this.debug.ProfilerEndFrame();
#endif
        }

        protected int IndexOf(TState state)
        {
            return this.states.FindIndex(x =>
            {
                return this.owner.Compare(x, state);
            });
        }
        #endregion

        #region IStateMachine<TState,TTrigger> Members
        public TState CurrentState
        {
            get { return this.states[this.currentState]; }
        }

        public TState NextState
        {
            get { return this.states[this.nextState]; }
        }

        public abstract bool IsRunning { get; }

        public abstract void Trigger(TTrigger trigger);
        public abstract void Restart();

        public void Advance()
        {
            if (IsRunning)
            {
                BeginFrame();
                {
                    this.owner.OnStateExecute(CurrentState);
                }
                EndFrame();

                GoToNextState();
            }
        }
        #endregion

        #region Setup
        private void Setup(ISimpleStateMachineOwner<TState, TTrigger> user, string stateProfilerFormat, string triggerProfilerFormat, TState[] initStates)
        {
            Setup(user, stateProfilerFormat, triggerProfilerFormat, new List<TState>(initStates));
        }

        protected virtual void Setup(ISimpleStateMachineOwner<TState, TTrigger> user, string stateProfilerFormat, string triggerProfilerFormat, List<TState> initStates)
        {
            if (!typeof(TState).IsEnum)
            {
                throw new ArgumentException("TState must be an enum type");
            }

            this.owner = user;
            this.states = initStates;

//todo #if ENABLE_PROFILER
//todo             this.debug.InitProfiling(stateProfilerFormat, triggerProfilerFormat, (stateTexts, stateFormat, triggerTexts, triggerFormat) =>
//todo             {
//todo                 EnumHelper.CacheEnum(stateTexts, stateFormat);
//todo                 EnumHelper.CacheEnum(triggerTexts, triggerFormat);
//todo             });
//todo #endif

            Restart();
        }
        #endregion
    }
}

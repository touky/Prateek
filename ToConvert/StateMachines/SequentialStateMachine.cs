namespace Assets.Prateek.ToConvert.StateMachines
{
    using System;
    using Assets.Prateek.ToConvert.StateMachines.Interfaces;
    using UnityEngine;

    /// <summary>
    ///     A simple sequential state machine that go through the states and callback your system to execute each state
    ///     Usage:
    ///     Declare a StateMachine with you enum as a state type.
    ///     Add the interface and ExecuteState/Compare callbacks to your class
    ///     Create it either by using the default CTor or one of the more custom ones
    ///     To update the state machine and go to the next state, just call Advance()
    ///     Use PreventStateChange = true to keep the state machine at this step, if you need to execute this state over
    ///     several frames
    ///     Use Restart()/JumpToEnd() to start/stop the state machine
    ///     Use ForceState(T state) if you need to go back to a previous state
    /// </summary>
    /// <typeparam name="TState">The enum type used for the state machine</typeparam>
    [Serializable]
    public class SequentialStateMachine<TState> : SimpleStateMachine<TState, SequentialTriggerType> where TState : struct, IConvertible
    {
        #region Fields
        private bool preventNextStateChange;
        #endregion

        #region Properties
        public override bool IsRunning
        {
            get { return currentState != nextState; }
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     This constructor will use all states in the enum from the first to the last as default
        ///     You can customize the profiler logging format with profilerFormat
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateProfilerFormat">String.Format for the profiler logging</param>
        /// <param name="triggerProfilerFormat">String.Format for the profiler logging</param>
        public SequentialStateMachine(ISequentialStateMachineOwner<TState> owner, string stateProfilerFormat = null, string triggerProfilerFormat = null)
            : base(owner, stateProfilerFormat, triggerProfilerFormat)
        {
            preventNextStateChange = false;
        }

        /// <summary>
        ///     This constructor will use the list given in stateSequence as the sequence of states
        ///     You can create two state machines with the same enum and different orders with this CTor
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateSequence">List of the sequence of states</param>
        public SequentialStateMachine(ISequentialStateMachineOwner<TState> owner, params TState[] stateSequence)
            : this(owner, null, null, stateSequence) { }

        /// <summary>
        ///     This constructor will use the list given in stateSequence as the sequence of states
        ///     You can create two state machines with the same enum and different orders with this CTor
        ///     You can customize the profiler logging format with profilerFormat
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stateProfilerFormat">String.Format for the profiler logging</param>
        /// <param name="stateSequence">List of the sequence of states</param>
        public SequentialStateMachine(ISequentialStateMachineOwner<TState> owner, string stateProfilerFormat, params TState[] stateSequence)
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
        public SequentialStateMachine(ISequentialStateMachineOwner<TState> owner, string stateProfilerFormat, string triggerProfilerFormat, params TState[] stateSequence)
            : base(owner, stateProfilerFormat, triggerProfilerFormat, stateSequence)
        {
            preventNextStateChange = false;
        }
        #endregion

        #region Class Methods
        /// <summary>
        ///     Restarts the state machine
        /// </summary>
        public override void Restart()
        {
            currentState = 0;
            nextState = 1;
        }

        protected override void GoToNextState()
        {
            if (!preventNextStateChange)
            {
                base.GoToNextState();

                nextState = Mathf.Clamp(nextState + 1, 0, states.Count - 1);
            }

            preventNextStateChange = false;
        }

        public override void Trigger(SequentialTriggerType trigger)
        {
            switch (trigger)
            {
                case SequentialTriggerType.PreventStateChange:
                {
                    preventNextStateChange = true;
                    break;
                }
                case SequentialTriggerType.JumpToEnd:
                {
                    nextState = states.Count - 1;
                    break;
                }
            }
        }

        public void Trigger(SequentialTriggerType trigger, TState state)
        {
            switch (trigger)
            {
                case SequentialTriggerType.ForceNextState:
                {
                    nextState = IndexOf(state);
                    break;
                }
            }
        }
        #endregion
    }
}

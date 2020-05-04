namespace Mayfair.Core.Code.StateMachines
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.StateMachines.Interfaces;

    /// <summary>
    ///     Flat state machine is a finite state machine with states, transitions & triggers to change between connected
    ///     states.
    ///     Since it inherits from SimpleStateMachine, States and triggers are enums, and it does not support inner state,
    ///     hence the "Flat" nickname.
    /// </summary>
    [Serializable]
    public class SimpleTriggerMachine<TState, TTrigger> : SimpleStateMachine<TState, TTrigger>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible
    {
        #region Fields
        private Dictionary<TState, Dictionary<TTrigger, TState>> triggers = new Dictionary<TState, Dictionary<TTrigger, TState>>();
        private int startState = -1;
        #endregion

        #region Properties
        public override bool IsRunning
        {
            get { return startState >= 0 && currentState >= 0; }
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
        /// <param name="startState">Starting state of the state machine</param>
        public SimpleTriggerMachine(ISimpleStateMachineOwner<TState, TTrigger> owner, TState startState, string stateProfilerFormat = null, string triggerProfilerFormat = null)
            : base(owner, stateProfilerFormat, triggerProfilerFormat)
        {
            this.startState = IndexOf(startState);
        }
        #endregion

        #region Class Methods
        public void SetTransition(TState source, TTrigger trigger, TState destination)
        {
            Dictionary<TTrigger, TState> transitions = null;
            if (!triggers.TryGetValue(source, out transitions))
            {
                transitions = new Dictionary<TTrigger, TState>();
                triggers.Add(source, transitions);
            }

            transitions[trigger] = destination;
        }

        public override void Restart()
        {
            currentState = startState;
            nextState = startState;
        }

        public override void Trigger(TTrigger trigger)
        {
            Dictionary<TTrigger, TState> transitions = null;
            if (!triggers.TryGetValue(CurrentState, out transitions))
            {
                owner.OnTrigger(trigger, false);
                return;
            }

            TState nextState = CurrentState;
            if (!transitions.TryGetValue(trigger, out nextState))
            {
                owner.OnTrigger(trigger, false);
                return;
            }

            owner.OnTrigger(trigger, true);
            this.nextState = IndexOf(nextState);
        }
        #endregion
    }
}

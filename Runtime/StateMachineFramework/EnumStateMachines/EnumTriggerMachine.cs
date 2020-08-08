namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Prateek.Runtime.StateMachineFramework.Interfaces;

    /// <summary>
    ///     EnumTriggerMachine is a state machine with enum states and triggers
    /// </summary>
    [Serializable]
    public abstract class EnumTriggerMachine<TState, TTrigger, TEnumComparer>
        : EnumStateMachine<TState, TTrigger, TEnumComparer>
        where TState : struct, IConvertible
        where TTrigger : struct, IConvertible
        where TEnumComparer : IEnumComparer<TState, TTrigger>, new()
    {
        #region Fields
        private Dictionary<TState, Dictionary<TTrigger, TState>> connections = new Dictionary<TState, Dictionary<TTrigger, TState>>();
        private int startState = -1;
        #endregion

        #region Properties
        public override bool IsActive
        {
            get { return startState >= 0 && activeState >= 0; }
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     EnumTriggerMachine is a state machine with enum states and triggers
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="startState"></param>
        protected EnumTriggerMachine(IEnumStateMachineOwner<TState> owner, TState startState)
            : base(owner)
        {
            this.startState = IndexOf(startState);
        }
        #endregion

        #region Class Methods
        public EnumTriggerMachine<TState, TTrigger, TEnumComparer> Connect(TState source, TTrigger trigger, TState destination)
        {
            if (!connections.TryGetValue(source, out var transitions))
            {
                transitions = new Dictionary<TTrigger, TState>();
                connections.Add(source, transitions);
            }

            transitions[trigger] = destination;

            return this;
        }

        public override void Reboot()
        {
            activeState = startState;
            incomingState = startState;
        }

        public override void Trigger(TTrigger trigger)
        {
            if (!connections.TryGetValue(ActiveState, out var transitions))
            {
                return;
            }

            var nextState = ActiveState;
            if (!transitions.TryGetValue(trigger, out nextState))
            {
                return;
            }

            incomingState = IndexOf(nextState);
        }
        #endregion
    }
}

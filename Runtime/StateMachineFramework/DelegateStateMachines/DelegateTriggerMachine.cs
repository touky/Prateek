namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using UnityEngine.Assertions;

    /// <summary>
    ///     EnumTriggerMachine is a state machine with enum states and triggers
    /// </summary>
    [Serializable]
    public abstract class DelegateTriggerMachine<TTrigger, TEnumComparer>
        : DelegateStateMachine<TTrigger, TEnumComparer>
        where TTrigger : struct, IConvertible
        where TEnumComparer : IEnumComparer<TTrigger>, new()
    {
        #region Fields
        private Dictionary<MethodInfo, Dictionary<TTrigger, MethodInfo>> connections = new Dictionary<MethodInfo, Dictionary<TTrigger, MethodInfo>>();
        private int startState = -1;
        #endregion

        #region Properties
        public override bool IsActive
        {
            get { return startState >= 0 && (incomingState >= 0 || activeState >= 0); }
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     EnumTriggerMachine is a state machine with enum states and triggers
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="startState"></param>
        protected DelegateTriggerMachine()
            : base()
        { }
        #endregion

        #region Class Methods
        public void Init(StateDelegate startState)
        {
            this.startState = IndexOf(startState.Method);
            Assert.IsTrue(this.startState != Const.INDEX_NONE, "Init() MUST be called at the end of the connection setup");
        }

        public DelegateTriggerMachine<TTrigger, TEnumComparer> Connect(StateDelegate source, TTrigger trigger, StateDelegate destination)
        {
            Add(source);
            Add(destination);

            var transitions = connections.SafeGet(source.Method);
            transitions[trigger] = destination.Method;

            return this;
        }

        public override void Reboot()
        {
            activeState = -1;
            Assert.IsTrue(this.startState != Const.INDEX_NONE, "Init() MUST be called at the end of the connection setup");
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
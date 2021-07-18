namespace Prateek.Runtime.StateMachineFramework.StandardStateMachines
{
    using System.Collections.Generic;
    using Prateek.Runtime.StateMachineFramework.DelegateStateMachines;
    using Prateek.Runtime.StateMachineFramework.Interfaces;
    using UnityEngine;

    public class StandardStateMachine<TTrigger>
        : StateMachine.IStateMachine<StandardState<TTrigger>, TTrigger>
    {
        #region Fields
        private StandardState<TTrigger> startState;
        private StandardState<TTrigger> activeState;
        private HashSet<StandardState<TTrigger>> states = new HashSet<StandardState<TTrigger>>();
        #endregion

        #region Constructors
        public StandardStateMachine(StandardState<TTrigger> startState)
        {
            Debug.Assert(startState != null);

            this.startState = startState;
            activeState = null;

            Add(startState);
        }
        #endregion

        #region Class Methods
        public void Add(StandardState<TTrigger> state)
        {
            if (!states.Contains(state))
            {
                states.Add(state);
                state.SetParent(this);
            }
        }

        public void Remove(StandardState<TTrigger> state)
        {
            if (states.Contains(state))
            {
                states.Remove(state);
                state.SetParent(null);
            }
        }
        #endregion

        #region IStateMachine<StandardState<TTrigger>,TTrigger> Members
        public StateMachine.IOwner Owner { get; private set; }

        public StandardState<TTrigger> ActiveState
        {
            get { return activeState; }
        }

        public StandardState<TTrigger> IncomingState
        {
            get { return activeState; }
        }

        public bool IsActive
        {
            get { return activeState != null; }
        }

        public void Step()
        {
            Debug.Assert(startState != null);

            if (activeState == null)
            {
                activeState = startState;
                activeState.StartState();
            }

            using (ProfilingScope.Open(activeState))
            {
                //Find the first triggered transition and consume it
                var transitions = activeState.Transitions;
                foreach (var transition in transitions)
                {
                    if (!transition.IsUsable || !transition.HasTriggered)
                    {
                        continue;
                    }

                    var targetState = transition.TryConsumingTrigger();
                    if (targetState == null)
                    {
                        continue;
                    }

                    //Stop the last state, and start the next one
                    activeState.StopState();
                    activeState = targetState;
                    activeState.StartState();

                    break;
                }

                //Execute the current active state
                activeState.Execute();
            }
        }

        public void Reboot()
        {
            if (activeState == null)
            {
                return;
            }

            activeState.StopState();
            activeState = null;
        }

        public void Trigger(TTrigger trigger)
        {
            if (activeState == null)
            {
                return;
            }

            activeState.Trigger(trigger);
        }
        #endregion

        #region IGadget
        public void Awake()
        {
            Reboot();
        }

        public void Kill()
        {
            activeState = null;
        }
        #endregion

        #region Nested type: ProfilingScope
        protected class ProfilingScope : StateMachineProfilingScope<StandardState<TTrigger>, TTrigger>
        {
            #region Constructors
            protected ProfilingScope(StandardState<TTrigger> state) : base(state.Name) { }
            #endregion
        }
        #endregion
    }
}

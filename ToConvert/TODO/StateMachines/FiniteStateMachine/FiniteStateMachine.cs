namespace Mayfair.Core.Code.StateMachines.FSM
{
    using System.Collections.Generic;
    using Interfaces;
    using UnityEngine;

    public class FiniteStateMachine<TTrigger> : IStateMachine<FiniteState<TTrigger>, TTrigger>
    {
        private const string NULL_STATE_NAME = "Null";

        #region Fields
        private FiniteState<TTrigger> startState;
        private FiniteState<TTrigger> activeState;
        private HashSet<FiniteState<TTrigger>> states = new HashSet<FiniteState<TTrigger>>();
#if ENABLE_PROFILER
        protected StateMachineDebug<FiniteState<TTrigger>, TTrigger> debug;
#endif
        #endregion

        #region Constructors
        public FiniteStateMachine(FiniteState<TTrigger> startState)
        {
            Debug.Assert(startState != null);

            this.startState = startState;
            this.activeState = null;

            Add(startState);

            BuildDebug();
        }
        #endregion

        #region Class Methods
        public void Add(FiniteState<TTrigger> state)
        {
            if (!this.states.Contains(state))
            {
                this.states.Add(state);
                state.SetParent(this);
            }

            BuildDebug();
        }

        public void Remove(FiniteState<TTrigger> state)
        {
            if (this.states.Contains(state))
            {
                this.states.Remove(state);
                state.SetParent(null);
            }

            BuildDebug();
        }

        public string GetCurrentStateName()
        {
            return CurrentState != null ? CurrentState.GetType().Name : NULL_STATE_NAME;
        }

        #region Debug
        private void BuildDebug()
        {
#if ENABLE_PROFILER
            this.debug.InitProfiling(null, null, (stateTexts, stateFormat, triggerTexts, triggerFormat) =>
            {
                foreach (FiniteState<TTrigger> state in this.states)
                {
                    stateTexts[state] = stateFormat == null ? state.ToString() : string.Format(stateFormat, state.ToString());
                }
            });
#endif
        }
        #endregion
        #endregion

        #region IStateMachine<FiniteState<TTrigger>,TTrigger> Members
        public FiniteState<TTrigger> CurrentState
        {
            get { return this.activeState; }
        }

        public FiniteState<TTrigger> NextState
        {
            get { return this.activeState; }
        }

        public bool IsRunning
        {
            get { return this.activeState != null; }
        }

        public void Advance()
        {
            Debug.Assert(this.startState != null);

            if (this.activeState == null)
            {
                this.activeState = this.startState;
                this.activeState.BeginState();
            }

#if ENABLE_PROFILER
            this.debug.ProfilerBeginFrame(CurrentState);
#endif
            {
                //Go through the transitions and try activating the triggered ones
                IReadOnlyList<FiniteState<TTrigger>.FiniteTransition> transitions = this.activeState.Transitions;
                foreach (FiniteState<TTrigger>.FiniteTransition transition in transitions)
                {
                    if (!transition.IsUsable)
                    {
                        continue;
                    }

                    if (!transition.HasTriggered)
                    {
                        continue;
                    }

                    FiniteState<TTrigger> targetState = transition.TryConsumingTrigger();
                    if (targetState == null)
                    {
                        continue;
                    }

                    this.activeState.EndState();
                    this.activeState = targetState;
                    this.activeState.BeginState();
                    break;
                }

                //Finally, execute the current
                this.activeState.Execute();
            }
#if ENABLE_PROFILER
            this.debug.ProfilerEndFrame();
#endif
        }

        public void Restart()
        {
            if (this.activeState == null)
            {
                return;
            }

            this.activeState.EndState();
            this.activeState = null;
        }

        public void Trigger(TTrigger trigger)
        {
            if (this.activeState == null)
            {
                return;
            }

            this.activeState.Trigger(trigger);
        }
        #endregion
    }
}

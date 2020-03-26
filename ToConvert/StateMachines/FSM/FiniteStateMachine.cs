namespace Assets.Prateek.ToConvert.StateMachines.FSM
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.StateMachines.Interfaces;
    using UnityEngine;

    public class FiniteStateMachine<TTrigger> : IStateMachine<FiniteState<TTrigger>, TTrigger>
    {
        #region Static and Constants
        private const string NULL_STATE_NAME = "Null";
        #endregion

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
            activeState = null;

            Add(startState);

            BuildDebug();
        }
        #endregion

        #region Class Methods
        public void Add(FiniteState<TTrigger> state)
        {
            if (!states.Contains(state))
            {
                states.Add(state);
                state.SetParent(this);
            }

            BuildDebug();
        }

        public void Remove(FiniteState<TTrigger> state)
        {
            if (states.Contains(state))
            {
                states.Remove(state);
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
            debug.InitProfiling(null, null, (stateTexts, stateFormat, triggerTexts, triggerFormat) =>
            {
                foreach (var state in states)
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
            get { return activeState; }
        }

        public FiniteState<TTrigger> NextState
        {
            get { return activeState; }
        }

        public bool IsRunning
        {
            get { return activeState != null; }
        }

        public void Advance()
        {
            Debug.Assert(startState != null);

            if (activeState == null)
            {
                activeState = startState;
                activeState.BeginState();
            }

#if ENABLE_PROFILER
            debug.ProfilerBeginFrame(CurrentState);
#endif
            {
                //Go through the transitions and try activating the triggered ones
                var transitions = activeState.Transitions;
                foreach (var transition in transitions)
                {
                    if (!transition.IsUsable)
                    {
                        continue;
                    }

                    if (!transition.HasTriggered)
                    {
                        continue;
                    }

                    var targetState = transition.TryConsumingTrigger();
                    if (targetState == null)
                    {
                        continue;
                    }

                    activeState.EndState();
                    activeState = targetState;
                    activeState.BeginState();
                    break;
                }

                //Finally, execute the current
                activeState.Execute();
            }
#if ENABLE_PROFILER
            debug.ProfilerEndFrame();
#endif
        }

        public void Restart()
        {
            if (activeState == null)
            {
                return;
            }

            activeState.EndState();
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
    }
}

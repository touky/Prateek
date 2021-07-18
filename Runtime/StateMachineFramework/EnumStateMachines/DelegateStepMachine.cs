namespace Prateek.Runtime.StateMachineFramework.EnumStateMachines
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    ///     An enum based step machine that will do
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
    public abstract class DelegateStepMachine
        : DelegateStateMachine<EnumStepTrigger, EnumStepTriggerComparer>
    {
        #region Fields
        private bool ignoreStateChange = false;
        #endregion

        #region Properties
        public override bool IsActive
        {
            get { return activeState != incomingState; }
        }
        #endregion

        #region Constructors
        /// <summary>
        ///     This constructor will use all states in the enum from the first to the last as default
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        protected DelegateStepMachine()
            : base() { }
        #endregion

        #region Class Methods
        /// <summary>
        ///     This method will use the list given in stateSequence as the sequence of states
        /// </summary>
        /// <param name="owner">The state machine owner</param>
        /// <param name="stepSequence">List of the sequence of states</param>
        public void Init(params StateDelegate[] stepSequence)
        {
            Init(new List<StateDelegate>(stepSequence));
        }

        /// <summary>
        ///     Reboot the state machine
        /// </summary>
        public override void Reboot()
        {
            activeState = 0;
            incomingState = 1;
        }

        protected override void TryChangingState()
        {
            if (!ignoreStateChange)
            {
                base.TryChangingState();

                incomingState = Mathf.Clamp(incomingState + 1, 0, states.Count - 1);
            }

            ignoreStateChange = false;
        }

        public override void Trigger(EnumStepTrigger trigger)
        {
            Trigger(trigger, default);
        }

        public void Trigger(EnumStepTrigger trigger, StateDelegate selectedState)
        {
            switch (trigger)
            {
                case EnumStepTrigger.IgnoreStateChange:
                {
                    ignoreStateChange = true;
                    break;
                }
                case EnumStepTrigger.SkipToEnd:
                {
                    incomingState = states.Count - 1;
                    break;
                }
                case EnumStepTrigger.SelectState:
                {
                    incomingState = IndexOf(selectedState.Method);
                    break;
                }
            }
        }
        #endregion
    }
}

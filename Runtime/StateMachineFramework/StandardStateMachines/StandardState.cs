namespace Prateek.Runtime.StateMachineFramework.StandardStateMachines
{
    using System.Collections.Generic;

    public abstract partial class StandardState<TTrigger>
    {
        #region Fields
        private string name;
        private bool enabled = true;
        private bool isActive = true;
        private StandardStateMachine<TTrigger> parent;
        private List<StandardTransition<TTrigger>> transitions = new List<StandardTransition<TTrigger>>();
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public virtual bool Enabled
        {
            get { return enabled; }
            protected set { enabled = value; }
        }

        protected bool IsActive
        {
            get { return isActive; }
        }

        public IReadOnlyList<StandardTransition<TTrigger>> Transitions
        {
            get { return transitions; }
        }
        #endregion

        #region Constructors
        protected StandardState()
        {
            name = GetType().Name;
        }
        #endregion

        #region Class Methods
        internal void SetParent(StandardStateMachine<TTrigger> parent)
        {
            if (parent == null)
            {
                this.parent = null;
                enabled = false;

                return;
            }

            //Prevent infinite loop
            if (this.parent == parent)
            {
                return;
            }

            this.parent = parent;
            foreach (var transition in transitions)
            {
                transition.TargetState.SetParent(parent);
            }
        }

        internal void AddTransition(StandardTransition<TTrigger> transition)
        {
            transitions.Add(transition);
        }

        public virtual void Trigger(TTrigger trigger)
        {
            foreach (var transition in transitions)
            {
                if (!transition.IsUsable)
                {
                    continue;
                }

                transition.Trigger(trigger);
            }
        }

        internal void StartState()
        {
            isActive = true;
            BeginState();
        }

        internal void Execute()
        {
            ExecuteState();
        }

        internal void StopState()
        {
            EndState();
            isActive = false;
        }

        protected abstract void BeginState();
        protected abstract void ExecuteState();
        protected abstract void EndState();
        #endregion
    }
}

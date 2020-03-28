namespace Mayfair.Core.Code.StateMachines.FSM
{
    using System.Collections.Generic;

    public abstract class FiniteState<TTrigger>
    {
        #region Fields
        private bool enabled = true;
        private bool isActive = true;
        private FiniteStateMachine<TTrigger> parent;
        private List<FiniteTransition> transitions = new List<FiniteTransition>();
        #endregion

        #region Properties
        public virtual bool Enabled
        {
            get { return this.enabled; }
            protected set { this.enabled = value; }
        }

        protected bool IsActive
        {
            get { return this.isActive; }
        }

        public IReadOnlyList<FiniteTransition> Transitions
        {
            get { return this.transitions; }
        }
        #endregion

        #region Class Methods
        public void SetParent(FiniteStateMachine<TTrigger> parent)
        {
            if (parent == null)
            {
                this.parent = null;
                this.enabled = false;

                return;
            }

            //Prevent infinite loop
            if (this.parent == parent)
            {
                return;
            }

            this.parent = parent;
            foreach (FiniteTransition transition in this.transitions)
            {
                transition.TargetState.SetParent(parent);
            }
        }

        private void AddTransition(FiniteTransition transition)
        {
            this.transitions.Add(transition);
        }

        public virtual void Trigger(TTrigger trigger)
        {
            foreach (FiniteTransition transition in this.transitions)
            {
                if (!transition.IsUsable)
                {
                    continue;
                }

                transition.TryTriggering(trigger);
            }
        }

        public void BeginState()
        {
            isActive = true;
            Begin();
        }

        public void EndState()
        {
            End();
            isActive = false;
        }

        protected abstract void Begin();
        public abstract void Execute();
        protected abstract void End();
        #endregion

        //This class is purposefully present here to hide the AddTransition member from the outside,
        //And ensure the transition.From().To() syntax is repected
        #region Nested type: FiniteTransition
        public abstract class FiniteTransition
        {
            #region Fields
            private FiniteState<TTrigger> targetState;
            #endregion

            #region Properties
            public abstract bool HasTriggered { get; }
            public abstract bool IsUsable { get; }

            public FiniteState<TTrigger> TargetState
            {
                get { return this.targetState; }
            }
            #endregion

            #region Class Methods
            public abstract void TryTriggering(TTrigger trigger);

            public FiniteTransition From(FiniteState<TTrigger> from)
            {
                from.AddTransition(this);
                return this;
            }

            public FiniteTransition To(FiniteState<TTrigger> to)
            {
                this.targetState = to;
                return this;
            }

            public virtual FiniteState<TTrigger> TryConsumingTrigger()
            {
                return this.TargetState;
            }
            #endregion
        }
        #endregion
    }
}

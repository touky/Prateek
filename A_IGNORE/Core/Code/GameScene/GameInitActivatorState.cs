namespace Mayfair.Core.Code.GameScene
{
    using System;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine;

    internal class GameInitActivatorState : FiniteState<bool>
    {
        #region Fields
        private GameInitActivator activator;
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            //Using this only because this state is used solely during the game init,
            // and not enough objects are present to risk a performance issue.
            activator = UnityEngine.Object.FindObjectOfType<GameInitActivator>();

            if (activator == null)
            {
                throw new Exception("No GameInitActivator in initServices !");
            }
        }

        public override void Execute()
        {
            if (activator.ActivationDone)
            {
                Trigger(true);
            }
        }

        protected override void End() { }
        #endregion
    }
}

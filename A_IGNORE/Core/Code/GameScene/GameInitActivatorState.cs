namespace Mayfair.Core.Code.GameScene
{
    using System;
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.StateMachines.FSM;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    internal class GameInitActivatorState : StandardState<bool>
    {
        #region Fields
        private GameInitActivator activator;
        #endregion

        #region Class Methods
        protected override void BeginState()
        {
            //Using this only because this state is used solely during the game init,
            // and not enough objects are present to risk a performance issue.
            activator = UnityEngine.Object.FindObjectOfType<GameInitActivator>();

            if (activator == null)
            {
                throw new Exception("No GameInitActivator in initServices !");
            }
        }

        protected override void ExecuteState()
        {
            if (activator.ActivationDone)
            {
                Trigger(true);
            }
        }

        protected override void EndState() { }
        #endregion
    }
}

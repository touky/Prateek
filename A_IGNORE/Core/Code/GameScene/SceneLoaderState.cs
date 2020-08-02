namespace Mayfair.Core.Code.GameScene
{
    using Mayfair.Core.Code.StateMachines.FSM;
    using Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal class SceneLoaderState : FiniteState<bool>
    {
        #region Fields
        private string scene;
        private bool setActive;
        #endregion

        #region Constructors
        public SceneLoaderState(string scene, bool setActive = false)
        {
            this.scene = scene;
            this.setActive = setActive;
        }
        #endregion

        #region Class Methods
        protected override void Begin()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(this.scene, LoadSceneMode.Additive);
            operation.completed += OnSceneLoaded;
        }

        public override void Execute() { }

        protected override void End()
        {
            if (this.setActive)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(this.scene));
            }
        }

        private void OnSceneLoaded(AsyncOperation operation)
        {
            Trigger(true);
        }
        #endregion
    }
}

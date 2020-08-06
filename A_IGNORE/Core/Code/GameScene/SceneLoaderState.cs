namespace Mayfair.Core.Code.GameScene
{
    using Mayfair.Core.Code.StateMachines.FSM;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    internal class SceneLoaderState : StandardState<bool>
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
        protected override void BeginState()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(this.scene, LoadSceneMode.Additive);
            operation.completed += OnSceneLoaded;
        }

        protected override void ExecuteState() { }

        protected override void EndState()
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

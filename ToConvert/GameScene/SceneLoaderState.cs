namespace Assets.Prateek.ToConvert.GameScene
{
    using Assets.Prateek.ToConvert.StateMachines.FSM;
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
            var operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            operation.completed += OnSceneLoaded;
        }

        public override void Execute() { }

        protected override void End()
        {
            if (setActive)
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
            }
        }

        private void OnSceneLoaded(AsyncOperation operation)
        {
            Trigger(true);
        }
        #endregion
    }
}

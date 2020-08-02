namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader.Enums;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class SceneReference : ContentHandle<SceneInstance, SceneReference>
    {
        #region Properties
        public SceneInstance Resource
        {
            get { return TypedContent; }
        }
        #endregion

        #region Constructors
        public SceneReference(ContentLoader loader) : base(loader)
        {
            loader.Behaviour = LoaderBehaviour.Scene;
        }
        #endregion

        #region Class Methods
        public override void LoadAsync()
        {
            content = default;
            loader.LoadCompleted = OnAsyncCompleted;

            InternalLoad();
        }

        public void UnloadAsync()
        {
            content = default;
            loader.LoadCompleted = OnAsyncCompleted;

            InternalUnload();
        }
        #endregion
    }
}

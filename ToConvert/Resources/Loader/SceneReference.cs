namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class SceneReference : AbstractResourceReference<SceneInstance, SceneReference>
    {
        #region Properties
        public SceneInstance Resource
        {
            get { return TypedResource; }
        }
        #endregion

        #region Constructors
        public SceneReference(ResourceLoader loader) : base(loader)
        {
            loader.Behaviour = ResourceLoader.LoaderBehaviour.Scene;
        }
        #endregion

        #region Class Methods
        public override void LoadAsync()
        {
            resource = default;
            loader.LoadCompleted = OnAsyncCompleted;

            InternalLoad();
        }

        public void UnloadAsync()
        {
            resource = default;
            loader.LoadCompleted = OnAsyncCompleted;

            InternalUnload();
        }
        #endregion
    }
}

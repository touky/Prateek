namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables.ContentLoaders;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class SceneHandle : ContentHandle<SceneInstance, SceneHandle>
    {
        #region Properties
        public SceneInstance Resource { get { return TypedContent; } }
        #endregion

        #region Constructors
        public SceneHandle(ContentLoader loader) : base(loader, false)
        {
            loader.SetLoadContextParameters(new AddressableLoaderParameters
            {
                behaviour = LoaderBehaviour.Scene
            });
        }
        #endregion
    }
}

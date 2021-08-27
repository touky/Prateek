namespace Prateek.Runtime.AppContentFramework.Unity.Handles
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables.ContentLoaders;
    using UnityEngine.ResourceManagement.ResourceProviders;

    public class SceneHandle
        : ContentHandle<SceneInstance>
    {
        #region Properties
        public SceneInstance Resource { get { return TypedContent; } }
        #endregion

        #region Class Methods
        protected override void OnInit(ContentLoader loader)
        {
            loader.SetLoadContextParameters(new AddressableLoaderParameters { behaviour = LoaderBehaviour.Scene });
        }
        #endregion
    }
}

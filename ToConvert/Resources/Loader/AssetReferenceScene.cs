namespace Assets.Prateek.ToConvert.Resources.Loader
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;

    //Since I don't have access to m_Operation, I need to have my own and override all the calls .....
    //
    //The idea is that the LoadSceneAsync() method IN THE OFFICIAL UNITY PLUGIN loads the scene as a single scene by default, and does not provide the LoadMode parameter.
    //It also does not give access to the m_Operation variable (because private) so I'm forced to copy the entire behaviour.
    //
    //SO, I created this class, and added this option.
    //It still retains the standard behaviour since it's inherited
    //
    //All overrided methods here are code copies of the unity one with this class operation variable used instead of the m_Operation one
    public class AssetReferenceScene : UnityEngine.AddressableAssets.AssetReference
    {
        #region Fields
        private AsyncOperationHandle operation;
        #endregion

        #region Properties
        public override Object Asset
        {
            get
            {
                if (!operation.IsValid())
                {
                    return null;
                }

                return operation.Result as Object;
            }
        }
        #endregion

        #region Constructors
        public AssetReferenceScene(string location) : base(location) { }
        #endregion

        #region Class Methods
        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(LoadSceneMode loadMode)
        {
            AsyncOperationHandle<SceneInstance> result = Addressables.LoadSceneAsync(RuntimeKey, loadMode);
            operation = result;
            return result;
        }

        public AsyncOperationHandle<SceneInstance> UnloadSceneAsync()
        {
            AsyncOperationHandle<SceneInstance> result = Addressables.UnloadSceneAsync(operation);
            operation = result;
            return result;
        }

        public override void ReleaseAsset()
        {
            if (!operation.IsValid())
            {
                //DebugTools.LogWarning("Cannot release a null or unloaded asset.");
                return;
            }

            Addressables.Release(operation);
            operation = default;
        }
        #endregion
    }
}

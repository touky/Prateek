namespace Mayfair.Core.Code.Resources.Loader
{
    using Mayfair.Core.Code.Utils.Debug;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    //Since I don't have access to m_Operation, I need to have my own and override all the calls .....
    //
    //The idea is that the LoadSceneAsync() method IN THE OFFICIAL UNITY PLUGIN loads the scene as a single scene by default, and does not provide the LoadMode parameter.
    //It also does not give access to the m_Operation variable (because private) so I'm forced to copy the entire behaviour.
    //
    //SO, I created this class, and added this option.
    //It still retains the standard behaviour since it's inherited
    //
    //All overrided methods here are code copies of the unity one with this class operation variable used instead of the m_Operation one
    public class AssetReferenceScene : AssetReference
    {
        #region Fields
        private AsyncOperationHandle operation;
        #endregion

        #region Properties
        //todo fix that
        //public override Object Asset
        //{
        //    get
        //    {
        //        if (!this.operation.IsValid())
        //        {
        //            return null;
        //        }

        //        return this.operation.Result as Object;
        //    }
        //}
        #endregion

        #region Constructors
        public AssetReferenceScene(string location) : base(location) { }
        #endregion

        #region Class Methods
        public AsyncOperationHandle<SceneInstance> LoadSceneAsync(LoadSceneMode loadMode)
        {
            AsyncOperationHandle<SceneInstance> result = Addressables.LoadSceneAsync(RuntimeKey, loadMode);
            this.operation = result;
            return result;
        }

        public AsyncOperationHandle<SceneInstance> UnloadSceneAsync()
        {
            AsyncOperationHandle<SceneInstance> result = Addressables.UnloadSceneAsync(this.operation);
            this.operation = result;
            return result;
        }

        //public override void ReleaseAsset()
        //{
        //    if (!this.operation.IsValid())
        //    {
        //        DebugTools.LogWarning("Cannot release a null or unloaded asset.");
        //        return;
        //    }

        //    Addressables.Release(this.operation);
        //    this.operation = default;
        //}
        #endregion
    }
}

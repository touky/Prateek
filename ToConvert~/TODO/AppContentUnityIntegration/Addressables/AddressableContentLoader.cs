namespace Mayfair.Core.Code.Resources.ServiceProvider {
    using System.Collections;
    using Mayfair.Core.Code.Resources.Enums;
    using Mayfair.Core.Code.Resources.Loader;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    internal class AddressableContentLoader : ContentLoader
    {
        #region Fields
        private AssetReference assetReference;
        private AsyncOperationHandle<Object> assetAsyncHandle;
        private AsyncOperationHandle<SceneInstance> sceneAsyncHandle;
        #endregion

        #region Properties
        public override float PercentComplete
        {
            get { return status == AsyncStatus.Nothing || !assetAsyncHandle.IsValid() ? 0 : assetAsyncHandle.PercentComplete; }
        }
        #endregion

        #region Constructors
        public AddressableContentLoader(string location) : base(location) { }
        #endregion

        #region Class Methods
        protected override void LoadAsset()
        {
            AssetReferenceT<Object> assetRef = new AssetReferenceT<Object>(location);
            assetReference = assetRef;
            assetAsyncHandle = assetRef.LoadAssetAsync();
            assetAsyncHandle.Completed += AddressableLoadCompleted;

            status = AsyncStatus.Loading;
        }

        protected override void UnloadAsset()
        {
            status = AsyncStatus.Nothing;
            if (assetReference != null)
            {
                assetReference.ReleaseAsset();
            }
        }

        private void AddressableLoadCompleted(AsyncOperationHandle<Object> handle)
        {
            AddressableLoadCompleted((IEnumerator) handle);
        }

        protected override void LoadSceneAsync()
        {
            AssetReferenceScene assetRef = new AssetReferenceScene(location);
            assetReference = assetRef;
            sceneAsyncHandle = assetRef.LoadSceneAsync(LoadSceneMode.Additive);
            sceneAsyncHandle.Completed += AddressableLoadCompleted;

            status = AsyncStatus.Loading;
        }

        protected override void UnloadSceneAsync()
        {
            if (!(assetReference is AssetReferenceScene))
            {
                return;
            }

            sceneAsyncHandle = (assetReference as AssetReferenceScene).UnloadSceneAsync();
            sceneAsyncHandle.Completed += AddressableLoadCompleted;

            status = AsyncStatus.Loading;
        }

        private void AddressableLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
        {
            AddressableLoadCompleted((IEnumerator) handle);
        }

        private void AddressableLoadCompleted(IEnumerator handle)
        {
            AsyncOperationStatus operationStatus = AsyncOperationStatus.None;
            if (handle is AsyncOperationHandle<Object>)
            {
                AsyncOperationHandle<Object> assetHandle = (AsyncOperationHandle<Object>) handle;
                operationStatus = assetHandle.Status;
                switch (assetHandle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                    {
                        content = assetHandle.Result;
                        break;
                    }
                }
            }
            else if (handle is AsyncOperationHandle<SceneInstance>)
            {
                AsyncOperationHandle<SceneInstance> sceneHandle = (AsyncOperationHandle<SceneInstance>) handle;
                operationStatus = sceneHandle.Status;
                switch (sceneHandle.Status)
                {
                    case AsyncOperationStatus.Succeeded:
                    {
                        content = sceneHandle.Result;
                        break;
                    }
                }
            }

            switch (operationStatus)
            {
                case AsyncOperationStatus.Failed:
                {
                    status = AsyncStatus.LoadingFailed;
                    break;
                }
                case AsyncOperationStatus.Succeeded:
                {
                    status = AsyncStatus.Loaded;
                    break;
                }
            }

            OnLoadCompleted();
        }
        #endregion
    }
}
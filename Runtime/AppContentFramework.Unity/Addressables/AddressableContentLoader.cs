namespace Prateek.Runtime.AppContentFramework.Unity.Addressables
{
    using System;
    using System.Collections;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.AppContentFramework.Loader.Enums;
    using UnityEngine.AddressableAssets;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    internal class AddressableContentLoader : ContentLoader
    {
        #region Fields
        private AssetReference assetReference;
        private AsyncOperationHandle<UnityEngine.Object> assetAsyncHandle;
        private AsyncOperationHandle<SceneInstance> sceneAsyncHandle;
        #endregion

        #region Properties
        public override float PercentComplete { get { return status == ContentAsyncStatus.Nothing || !assetAsyncHandle.IsValid() ? 0 : assetAsyncHandle.PercentComplete; } }
        #endregion

        #region Constructors
        public AddressableContentLoader(string path) : base(path) { }
        #endregion

        #region Class Methods
        protected override void Load(LoaderParameters parameters)
        {
            if (!(parameters is AddressableContextParameters aParameters))
            {
                throw new Exception($"{nameof(LoaderParameters)} are of the wrong type: {parameters.GetType().Name}");
            }

            switch (aParameters.behaviour)
            {
                case LoaderBehaviour.Asset:
                {
                    LoadAsset();
                    break;
                }
                case LoaderBehaviour.Scene:
                {
                    LoadSceneAsync(aParameters);
                    break;
                }
            }
        }

        protected override void Unload(LoaderParameters parameters)
        {
            if (!(parameters is AddressableContextParameters aParameters))
            {
                throw new Exception($"{nameof(LoaderParameters)} are of the wrong type: {parameters.GetType().Name}");
            }

            switch (aParameters.behaviour)
            {
                case LoaderBehaviour.Asset:
                {
                    UnloadAsset();
                    break;
                }
                case LoaderBehaviour.Scene:
                {
                    UnloadSceneAsync();
                    break;
                }
            }
        }

        protected void LoadAsset()
        {
            var assetRef = new AssetReferenceT<UnityEngine.Object>(path);
            assetReference = assetRef;
            assetAsyncHandle = assetRef.LoadAssetAsync();
            assetAsyncHandle.Completed += AddressableLoadCompleted;

            status = ContentAsyncStatus.Loading;
        }

        protected void UnloadAsset()
        {
            status = ContentAsyncStatus.Nothing;
            if (assetReference != null)
            {
                assetReference.ReleaseAsset();
            }
        }

        protected void LoadSceneAsync(AddressableContextParameters aParameters)
        {
            var assetRef = new AssetReference(path);
            assetReference = assetRef;
            sceneAsyncHandle = assetRef.LoadSceneAsync(LoadSceneMode.Additive, false);
            sceneAsyncHandle.Completed += AddressableLoadCompleted;

            status = ContentAsyncStatus.Loading;
        }

        protected void UnloadSceneAsync()
        {
            sceneAsyncHandle = assetReference.UnLoadScene();
            sceneAsyncHandle.Completed += AddressableLoadCompleted;

            status = ContentAsyncStatus.Loading;
        }

        private void AddressableLoadCompleted(AsyncOperationHandle<UnityEngine.Object> handle)
        {
            AddressableLoadCompleted((IEnumerator) handle);
        }

        private void AddressableLoadCompleted(AsyncOperationHandle<SceneInstance> handle)
        {
            AddressableLoadCompleted((IEnumerator) handle);
        }

        private void AddressableLoadCompleted(IEnumerator handle)
        {
            var operationStatus = AsyncOperationStatus.None;
            if (handle is AsyncOperationHandle<UnityEngine.Object>)
            {
                var assetHandle = (AsyncOperationHandle<UnityEngine.Object>) handle;
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
                var sceneHandle = (AsyncOperationHandle<SceneInstance>) handle;
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
                    status = ContentAsyncStatus.Failed;
                    break;
                }
                case AsyncOperationStatus.Succeeded:
                {
                    status = ContentAsyncStatus.Loaded;
                    break;
                }
            }

            OnLoadCompleted();
        }
        #endregion
    }
}

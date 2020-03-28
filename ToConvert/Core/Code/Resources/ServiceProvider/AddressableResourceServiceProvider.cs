namespace Mayfair.Core.Code.Resources.ServiceProvider
{
    using System.Collections;
    using Mayfair.Core.Code.Resources.Enums;
    using Mayfair.Core.Code.Resources.Loader;
    using Mayfair.Core.Code.StateMachines;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.AddressableAssets.ResourceLocators;
    using UnityEngine.ResourceManagement.AsyncOperations;
    using UnityEngine.ResourceManagement.ResourceProviders;
    using UnityEngine.SceneManagement;

    public class AddressableResourceServiceProvider : ResourceServiceProvider
    {
        #region Fields
        private bool addressSystemInitialized = false;
        #endregion

        #region Properties
        public override bool IsProviderValid
        {
            //TODO: re-inject this.addressSystemInitialized within this logic
            get { return true; }
        }

        public override int Priority
        {
            get { return 0; }
        }
        #endregion

        #region Class Methods
        public override void ExecuteState(ResourceService service, ServiceState state)
        {
            switch (state)
            {
                case ServiceState.Init:
                {
                    Addressables.InitializeAsync().Completed += InitCompleted;
                    break;
                }
                case ServiceState.InitWait:
                {
                    if (!addressSystemInitialized)
                    {
                        service.Trigger(SequentialTriggerType.PreventStateChange);
                    }

                    break;
                }
                case ServiceState.ResourceTriage:
                {
                    //Not very nice ....
                    foreach (IResourceLocator locator in Addressables.ResourceLocators)
                    {
                        foreach (object key in locator.Keys)
                        {
                            //Yes this cast should stay like this or an assert may happen if said cast
                            // is performed within the foreach
                            string location = key as string;
                            if (location == null)
                            {
                                continue;
                            }

                            service.Store(new AddressableResourceLoader(location));
                        }
                    }

                    break;
                }
            }
        }

        private void InitCompleted(AsyncOperationHandle<IResourceLocator> obj)
        {
            addressSystemInitialized = true;
        }
        #endregion

        #region Nested type: AddressableResourceLoader
        private class AddressableResourceLoader : ResourceLoader
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
            public AddressableResourceLoader(string location) : base(location) { }
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
                            resource = assetHandle.Result;
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
                            resource = sceneHandle.Result;
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
        #endregion
    }
}

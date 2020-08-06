namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration.Addressables
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Daemons;
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;
    using Prateek.Runtime.StateMachineFramework.EnumStateMachines;
    using UnityEngine.AddressableAssets;
    using UnityEngine.AddressableAssets.ResourceLocators;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AddressableRegistryServant
        : ContentRegistryServant
    {
        #region Fields
        private bool addressSystemInitialized = false;
        #endregion

        #region Properties
        public override bool IsAlive
        {
            //TODO: re-inject this.addressSystemInitialized within this logic
            get { return base.IsAlive; }
        }
        #endregion

        #region Class Methods
        public override void ExecuteState(ContentRegistryDaemon daemonCore, ServiceState state)
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
                        daemonCore.Trigger(EnumStepTrigger.IgnoreStateChange);
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

                            daemonCore.Store(new AddressableContentLoader(location));
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
    }
}

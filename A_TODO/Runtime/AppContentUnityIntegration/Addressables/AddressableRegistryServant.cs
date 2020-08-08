namespace Prateek.A_TODO.Runtime.AppContentUnityIntegration.Addressables
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Daemons;
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;
    using Prateek.A_TODO.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.StateMachineFramework.EnumStateMachines;
    using UnityEngine.AddressableAssets;
    using UnityEngine.AddressableAssets.ResourceLocators;
    using UnityEngine.ResourceManagement.AsyncOperations;

    public class AddressableRegistryServant
        : ContentRegistryServant
    {
        #region Fields
        private bool addressSystemInitialized = false;
        private bool workPending = false;
        #endregion

        #region Properties
        public override bool IsAlive
        {
            get { return base.IsAlive && addressSystemInitialized; }
        }
        #endregion

        #region Class Methods
        public override void ExecutingState(State state)
        {
            switch (state)
            {
                case State.Startup:
                {
                    Addressables.InitializeAsync().Completed += InitCompleted;
                    break;
                }
                case State.Idle:
                {
                    if (workPending)
                    {
                        workPending = false;

                        WorkIsReady();
                    }
                    break;
                }
                case State.StartWork:
                {
                    InvalidateAllPaths();
                    break;
                }
                case State.Working:
                {
                    //Go throught the locators and store them in the overseer
                    foreach (IResourceLocator locator in Addressables.ResourceLocators)
                    {
                        foreach (object key in locator.Keys)
                        {
                            //The null check can NullRef if called from the foreach
                            string location = key as string;
                            if (location == null)
                            {
                                continue;
                            }

                            ValidatePath(location);
                        }
                    }

                    break;
                }
            }

            base.ExecutingState(state);
        }

        protected override ContentLoader GetNewContentLoader(string path)
        {
            throw new System.NotImplementedException();
        }

        private void InitCompleted(AsyncOperationHandle<IResourceLocator> obj)
        {
            addressSystemInitialized = true;
            workPending = true;
        }
        #endregion
    }
}

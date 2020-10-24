namespace Prateek.Runtime.AppContentFramework.Unity.Addressables
{
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.AppContentFramework.Loader;
    using Prateek.Runtime.DebugFramework.DebugMenu;
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
                    foreach (var locator in Addressables.ResourceLocators)
                    {
                        foreach (var key in locator.Keys)
                        {
                            //The null check can NullRef if called from the foreach
                            var location = key as string;
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
            return new AddressableContentLoader(path);
        }

        private void InitCompleted(AsyncOperationHandle<IResourceLocator> obj)
        {
            addressSystemInitialized = true;
            workPending = true;
        }

        public override void SetupDebugDocument(DebugMenuDocument document)
        {
            var section = new AddressableRegistrySection(this, "Addressable Servant");

            document.AddSections(section);
        }
        #endregion
    }
}

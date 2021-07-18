namespace Prateek.Runtime.AppContentFramework.Unity.Addressables
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Unity.Addressables.Debug;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
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
            get { return base.IsAlive && addressSystemInitialized; }
        }
        #endregion

        #region Class Methods
        protected override void StartWork(StateStatus stateStatus)
        {
            if (stateStatus == StateStatus.Execute)
            {
                Addressables.InitializeAsync().Completed += InitCompleted;
            }

            base.StartWork(stateStatus);
        }

        protected override void Working(StateStatus stateStatus)
        {
            if (stateStatus == StateStatus.Execute)
            {
                //Go through the locators and store them in the overseer
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

                workStatus = WorkStatus.Done;
            }

            base.StartWork(stateStatus);
        }

        protected override ContentLoader GetNewContentLoader(string path)
        {
            return new AddressableContentLoader(path);
        }

        private void InitCompleted(AsyncOperationHandle<IResourceLocator> obj)
        {
            addressSystemInitialized = true;
            workStatus = WorkStatus.Pending;
        }

        public override void SetupDebugDocument(DebugMenuDocument document)
        {
            var section = new AddressableRegistrySection(this, "Addressable Servant");

            document.AddSections(section);
        }
        #endregion
    }
}

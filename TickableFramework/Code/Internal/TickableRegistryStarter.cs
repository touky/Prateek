namespace Prateek.TickableFramework.Code.Internal
{
    using System.Collections.Generic;
    using Prateek.TickableFramework.Code.Interfaces;

    internal abstract class TickableRegistryStarter : TickableRegistryHelper
    {
        #region Fields
        private List<ITickable> tickalesToStart = new List<ITickable>();
        #endregion

        #region Unity Methods
        //---------------------------------------------------------------------
        private void Start()
        {
            registry.OnStarterStart();
        }
        #endregion
    }
}

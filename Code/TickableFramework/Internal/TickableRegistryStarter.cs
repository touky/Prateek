namespace Prateek.TickableFramework.Code.Internal
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Prateek.TickableFramework.Code.Interfaces;

    internal abstract class TickableRegistryStarter : TickableRegistryHelper
    {
        #region Fields
        private List<ITickable> tickalesToStart = new List<ITickable>();
        #endregion

        #region Unity Methods
        ///---------------------------------------------------------------------
        [UsedImplicitly]
        private void Start()
        {
            registry.StartTickables();
        }
        #endregion
    }
}

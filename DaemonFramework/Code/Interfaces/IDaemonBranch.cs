namespace Prateek.DaemonCore.Code.Interfaces
{
    using System;
    using Mayfair.Core.Code.Utils.Types.Priority;
    using Prateek.TickableFramework.Code.Interfaces;

    public interface IDaemonBranch
        : IPriority
    {
        #region Properties
        void Startup();
        void Shutdown();

        String Name { get; }
        bool IsAlive { get; }
        #endregion
    }
}

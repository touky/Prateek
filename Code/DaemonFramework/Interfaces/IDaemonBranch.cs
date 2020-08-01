namespace Prateek.DaemonFramework.Code.Interfaces
{
    using Prateek.Core.Code.Interfaces.IPriority;
    using System;

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

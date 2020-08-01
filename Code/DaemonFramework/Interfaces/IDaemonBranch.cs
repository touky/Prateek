namespace Prateek.DaemonFramework.Code.Interfaces
{
    using Prateek.Core.Code.Interfaces.IPriority;
    using System;

    public interface IServant
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

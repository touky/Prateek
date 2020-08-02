namespace Prateek.Runtime.DaemonFramework.Interfaces
{
    using Prateek.Runtime.Core.Interfaces.IPriority;

    public interface IServant
        : IPriority
    {
        #region Properties
        string Name { get; }
        bool IsAlive { get; }
        #endregion

        #region Class Methods
        void Startup();
        void Shutdown();
        #endregion
    }
}

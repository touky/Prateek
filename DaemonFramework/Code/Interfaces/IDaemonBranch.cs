namespace Prateek.DaemonCore.Code.Interfaces
{
    using System;
    using Mayfair.Core.Code.Utils.Types.Priority;
    using Prateek.DaemonCore.Code.Enums;

    public interface IDaemonBranch : IPriority
    {
        #region Properties
        void Startup();
        void Shutdown();

        String Name { get; }
        bool IsAlive { get; }
        #endregion
    }

    internal static class DeamonUtils
    {
        #region Class Methods
        internal static void ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction action, TDaemonBranch branch)
            where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
            where TDaemonBranch : class, IDaemonBranch
        {
            DaemonCore<TDaemonCore, TDaemonBranch>.ChangeStatus(action, branch);
        }
        #endregion
    }
}

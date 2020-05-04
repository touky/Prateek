namespace Prateek.DaemonFramework.Code.Interfaces
{
    using Prateek.DaemonFramework.Code.Enums;

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

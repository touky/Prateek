namespace Prateek.DaemonCore.Code.Interfaces
{
    using Prateek.DaemonCore.Code.Enums;

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

namespace Prateek.DaemonFramework.Code.Interfaces
{
    using Prateek.DaemonFramework.Code.Enums;

    internal static class DeamonUtils
    {
        #region Class Methods
        internal static void ChangeStatus<TDaemon, TServant>(StatusAction action, TServant servant)
            where TDaemon : Daemon<TDaemon, TServant>
            where TServant : class, IServant
        {
            Daemon<TDaemon, TServant>.ChangeStatus(action, servant);
        }
        #endregion
    }
}

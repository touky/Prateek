namespace Prateek.DaemonCore.Code.Branches
{
    using Prateek.DaemonCore.Code.Enums;
    using Prateek.DaemonCore.Code.Interfaces;

    public abstract class DaemonBranch<TDaemonCore, TDaemonBranch> : DaemonBranch
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region Class Methods
        public override void Startup()
        {
            DeamonUtils.ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction.Register, this as TDaemonBranch);
        }

        public override void Shutdown()
        {
            DeamonUtils.ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction.Unregister, this as TDaemonBranch);
        }
        #endregion
    }
}

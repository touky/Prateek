namespace Prateek.DaemonCore.Code.Branches
{
    using Prateek.DaemonCore.Code.Enums;
    using Prateek.DaemonCore.Code.Interfaces;

    public abstract class DaemonBranch<TDaemonCore, TDaemonBranch> : IDaemonBranch
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region IDaemonBranch Members
        public abstract string Name { get; }
        public abstract bool IsAlive { get; }
        public abstract int Priority { get; }
        #endregion

        #region Class Methods
        public virtual void Startup()
        {
            DeamonUtils.ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction.Register, this as TDaemonBranch);
        }

        public virtual void Shutdown()
        {
            DeamonUtils.ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction.Unregister, this as TDaemonBranch);
        }
        #endregion
    }
}

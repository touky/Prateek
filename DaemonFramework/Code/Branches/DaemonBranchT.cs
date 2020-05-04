namespace Prateek.DaemonFramework.Code.Branches
{
    using Prateek.DaemonFramework.Code.Enums;
    using Prateek.DaemonFramework.Code.Interfaces;

    public abstract class DaemonBranch<TDaemonCore, TDaemonBranch>
        : IDaemonBranch
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region IDaemonBranch Members
        public virtual string Name
        {
            get { return "NOT_IMPLEMENTED"; }
        }

        public virtual bool IsAlive
        {
            get { return false; }
        }

        public virtual int Priority
        {
            get { return 0; }
        }

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

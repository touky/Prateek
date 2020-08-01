namespace Prateek.DaemonFramework.Code.Servants
{
    using Prateek.DaemonFramework.Code.Enums;
    using Prateek.DaemonFramework.Code.Interfaces;

    public abstract class Servant<TDaemon, TServant>
        : IServant
        where TDaemon : Daemon<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region IServant Members
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
            DeamonUtils.ChangeStatus<TDaemon, TServant>(StatusAction.Register, this as TServant);
        }

        public virtual void Shutdown()
        {
            DeamonUtils.ChangeStatus<TDaemon, TServant>(StatusAction.Unregister, this as TServant);
        }
        #endregion
    }
}

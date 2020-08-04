namespace Prateek.Runtime.DaemonFramework.Servants
{
    using System.Security.Cryptography;
    using Prateek.Runtime.DaemonFramework.Enums;
    using Prateek.Runtime.DaemonFramework.Interfaces;

    public abstract class Servant<TDaemon, TServant>
        : IServant, IServantInternal
        where TDaemon : DaemonOverseer<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Fields
        private TDaemon overseer;
        private string name = null;
        #endregion

        #region Properties
        protected TDaemon Overseer
        {
            get { return overseer; }
        }
        #endregion

        #region IServant Members
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                {
                    name = $"Servant<{typeof(TDaemon).Name}, {typeof(TServant).Name}>";
                }

                return name;
            }
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
            DaemonOverseer<TDaemon, TServant>.SetServantStatus(this as TServant, StatusAction.Register);
        }

        public virtual void Shutdown()
        {
            DaemonOverseer<TDaemon, TServant>.SetServantStatus(this as TServant, StatusAction.Unregister);
        }
        #endregion

        #region IServantInternal Members
        string IServantInternal.Name
        {
            set { name = value; }
        }

        IDaemon IServantInternal.Overseer
        {
            set { overseer = value as TDaemon; }
        }
        #endregion
    }
}

namespace Prateek.Runtime.DaemonFramework.Servants
{
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework.Enums;
    using Prateek.Runtime.DaemonFramework.Interfaces;

    /// <summary>
    /// Servants are tied to the <see cref="DaemonOverseer{TDaemon,TServant}"/> system meant to represent a
    /// specialization of the Overseer behaviour.
    /// They implement the <see cref="IPriority"/> interface to be ordered in the overseer access list
    /// </summary>
    /// <typeparam name="TDaemon">The Deamon class type class is registering to</typeparam>
    /// <typeparam name="TServant">The Servant class type inheriting from this class</typeparam>
    public abstract class Servant<TDaemon, TServant>
        : IServant
        , IServantInternal
        , IPriority
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
            get { return true; }
        }

        public virtual int DefaultPriority
        {
            get { return Overseer.DefaultPriority + 1; }
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

namespace Prateek.DaemonFramework.Code.Servants
{
    using Prateek.DaemonFramework.Code.Enums;
    using Prateek.DaemonFramework.Code.Interfaces;
    using UnityEngine;

    public abstract class ServantBehaviour<TDaemon, TServant>
        : MonoBehaviour, IServant
        where TDaemon : Daemon<TDaemon, TServant>
        where TServant : class, IServant
    {
        #region Unity Methods
        private void Awake()
        {
            Startup();
        }

        private void OnDestroy()
        {
            Shutdown();
        }
        #endregion

        #region IServant Members
        public virtual string Name
        {
            get { return name; }
        }

        public virtual bool IsAlive
        {
            get { return enabled; }
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

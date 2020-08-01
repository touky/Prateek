namespace Prateek.DaemonFramework.Code.Branches
{
    using Prateek.DaemonFramework.Code.Enums;
    using Prateek.DaemonFramework.Code.Interfaces;
    using UnityEngine;

    public abstract class DaemonBranchBehaviour<TDaemonCore, TDaemonBranch>
        : MonoBehaviour, IDaemonBranch
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
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

        #region IDaemonBranch Members
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
            DeamonUtils.ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction.Register, this as TDaemonBranch);
        }

        public virtual void Shutdown()
        {
            DeamonUtils.ChangeStatus<TDaemonCore, TDaemonBranch>(StatusAction.Unregister, this as TDaemonBranch);
        }
        #endregion
    }
}

namespace Prateek.DaemonCore.Code.Branches
{
    using Prateek.DaemonCore.Code.Enums;
    using Prateek.DaemonCore.Code.Interfaces;
    using UnityEngine;

    public abstract class DaemonBranchBehaviour<TDaemonCore, TDaemonBranch> : MonoBehaviour, IDaemonBranch
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region IDaemonBranch Members
        public virtual string Name  { get { return name; } }
        public virtual bool IsAlive { get { return enabled; } }
        public virtual int Priority { get { return 0; } }
        #endregion

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

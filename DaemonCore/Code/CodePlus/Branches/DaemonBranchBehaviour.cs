namespace Prateek.DaemonCore.Code.Branches
{
    using Prateek.DaemonCore.Code.Interfaces;
    using UnityEngine;

    public abstract class DaemonBranchBehaviour : MonoBehaviour, IDaemonBranch
    {
        #region Properties
        protected abstract bool IsAliveInternal { get; }
        #endregion

        #region IDaemonBranch Members
        public bool IsAlive
        {
            get { return enabled && IsAliveInternal; }
        }

        public abstract int Priority { get; }
        #endregion
    }
}

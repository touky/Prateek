namespace Prateek.DaemonFramework.Code
{
    using System;
    using System.Collections.Generic;
    using Prateek.Core.Code.Singleton;
    using Prateek.DaemonFramework.Code.Enumerators;
    using Prateek.DaemonFramework.Code.Enums;
    using Prateek.DaemonFramework.Code.Interfaces;

    public abstract class DaemonCore<TDaemonCore, TDaemonBranch>
        : SingletonBehaviour<TDaemonCore>, IDaemonCore<TDaemonBranch>, ITickable
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region Fields
        private List<TDaemonBranch> branches = new List<TDaemonBranch>();
        #endregion

        #region Class Methods
        internal static void ChangeStatus(StatusAction action, TDaemonBranch branch)
        {
            IDaemonCore<TDaemonBranch> instance = Instance;

            //This will only happen when OnApplicationQuit has been called
            if (instance == null)
            {
                return;
            }

            switch (action)
            {
                case StatusAction.Register:
                {
                    instance.Register(branch);
                    break;
                }
                case StatusAction.Unregister:
                {
                    instance.Unregister(branch);
                    break;
                }
                default:
                {
                    throw new Exception($"{branch.GetType().Name} sent idenfication without the action setup.");
                }
            }
        }

        protected TDaemonBranch GetFirstAliveBranch()
        {
            foreach (var branch in branches)
            {
                if (branch.IsAlive)
                {
                    return branch;
                }
            }

            return default;
        }

        protected IEnumerable<TDaemonBranch> GetValidBranches(bool includeInvalid = false)
        {
            return new DaemonBranchEnumerable<TDaemonBranch>(branches, includeInvalid);
        }

        protected virtual void OnBranchRegistered(TDaemonBranch branch) { }
        protected virtual void OnBranchUnregistered(TDaemonBranch branch) { }
        #endregion

        #region IDaemonCore<TDaemonBranch> Members
        void IDaemonCore<TDaemonBranch>.Register(TDaemonBranch branch)
        {
            if (branches.Contains(branch))
            {
                return;
            }

            branches.Add(branch);
            branches.SortWithPriorities();

            OnBranchRegistered(branch);
        }

        void IDaemonCore<TDaemonBranch>.Unregister(TDaemonBranch branch)
        {
            if (!branches.Contains(branch))
            {
                return;
            }

            branches.Remove(branch);
            branches.SortWithPriorities();

            OnBranchUnregistered(branch);
        }
        #endregion

        #region ITickable Members
        public virtual int Priority
        {
            get { return 0; }
        }

        public virtual TickableSetup TickableSetup
        {
            get { return TickableSetup.Nothing; }
        }

        public virtual void InitializeTickable() { }

        public virtual void TickFixed(TickableFrame tickableFrame, float seconds) { }

        public virtual void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds) { }

        public virtual void TickLate(TickableFrame tickableFrame, float seconds) { }

        public virtual void ApplicationIsQuitting() { }

        public virtual void ApplicationChangeFocus(bool appStatus) { }

        public virtual void ApplicationChangePause(bool appStatus) { }

        public virtual void DrawGUI() { }
        #endregion
    }
}

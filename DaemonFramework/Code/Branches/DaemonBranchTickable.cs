namespace Prateek.DaemonFramework.Code.Branches
{
    using Prateek.DaemonFramework.Code.Interfaces;
    using Prateek.TickableFramework.Code.Enums;
    using Prateek.TickableFramework.Code.Interfaces;

    public abstract class DaemonBranchTickable<TDaemonCore, TDaemonBranch>
        : DaemonBranch<TDaemonCore, TDaemonBranch>, ITickable
        where TDaemonCore : DaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region ITickable Members
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

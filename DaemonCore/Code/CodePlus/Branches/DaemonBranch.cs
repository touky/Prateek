namespace Prateek.DaemonCore.Code.Branches
{
    using Prateek.DaemonCore.Code.Interfaces;

    public abstract class DaemonBranch : IDaemonBranch
    {
        #region Class Methods
        public abstract void Startup();
        public abstract void Shutdown();
        #endregion

        #region IDaemonBranch Members
        public abstract bool IsAlive { get; }
        public abstract int Priority { get; }
        #endregion
    }
}

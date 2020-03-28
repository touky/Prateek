namespace Prateek.DaemonCore.Code.Interfaces
{
    public interface IDaemonCore<TDaemonBranch> : IDaemonCore
        where TDaemonBranch : IDaemonBranch
    {
        #region Service
        void Register(TDaemonBranch branch);
        #endregion

        #region Class Methods
        void Unregister(TDaemonBranch branch);
        #endregion
    }
}

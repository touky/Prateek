namespace Prateek.DaemonFramework.Code.Interfaces
{
    public interface IDaemonCore<TDaemonBranch> : IDaemonCore
        where TDaemonBranch : IDaemonBranch
    {
        #region Registering
        void Register(TDaemonBranch branch);
        void Unregister(TDaemonBranch branch);
        #endregion
    }
}

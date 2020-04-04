namespace Prateek.DaemonCore.Code.Interfaces
{
    using UnityEngine.XR.WSA.Persistence;

    public interface IDaemonCore<TDaemonBranch> : IDaemonCore
        where TDaemonBranch : IDaemonBranch
    {
        #region Registering
        void Register(TDaemonBranch branch);
        void Unregister(TDaemonBranch branch);
        #endregion
    }
}

namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Interfaces;

    public abstract class ServiceState<TTrigger, TDaemonCore> : EmptyState<TTrigger>
        where TDaemonCore : IDaemonCore
    {
        #region Fields
        protected TDaemonCore daemonCore;
        #endregion

        #region Constructors
        protected ServiceState(TDaemonCore daemonCore)
        {
            this.daemonCore = daemonCore;
        }
        #endregion
    }
}

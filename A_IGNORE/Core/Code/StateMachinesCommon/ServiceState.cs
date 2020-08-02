namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Service;
    using Prateek.A_TODO.Runtime.StateMachines.FiniteStateMachine.Common;
    using Prateek.Runtime.DaemonFramework.Interfaces;

    public abstract class ServiceState<TTrigger, TDaemon> : EmptyState<TTrigger>
        where TDaemon : IDaemon
    {
        #region Fields
        protected TDaemon daemonCore;
        #endregion

        #region Constructors
        protected ServiceState(TDaemon daemonCore)
        {
            this.daemonCore = daemonCore;
        }
        #endregion
    }
}

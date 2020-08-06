namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Service;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.StateMachineFramework.StandardStateMachines;

    public abstract class ServiceState<TTrigger, TDaemon> : IdleState<TTrigger>
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

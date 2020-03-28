namespace Mayfair.Core.Code.StateMachines.FSM.Common
{
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Code.Service.Interfaces;

    public abstract class ServiceState<TTrigger, TService> : EmptyState<TTrigger>
        where TService : IService
    {
        #region Fields
        protected TService service;
        #endregion

        #region Constructors
        protected ServiceState(TService service)
        {
            this.service = service;
        }
        #endregion
    }
}

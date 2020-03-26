namespace Assets.Prateek.ToConvert.StateMachines.FSM.Common
{
    using Assets.Prateek.ToConvert.Service.Interfaces;

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

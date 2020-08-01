namespace Prateek.DaemonFramework.Code.Interfaces
{
    public interface IDaemon<TServant> : IDaemon
        where TServant : IServant
    {
        #region Registering
        void Register(TServant servant);
        void Unregister(TServant servant);
        #endregion
    }
}

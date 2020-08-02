namespace Prateek.Runtime.DaemonFramework.Interfaces
{
    public interface IDaemonOverseer<TServant> : IDaemon
        where TServant : IServant
    {
        #region Registering
        void Register(TServant servant);
        void Unregister(TServant servant);
        #endregion
    }
}

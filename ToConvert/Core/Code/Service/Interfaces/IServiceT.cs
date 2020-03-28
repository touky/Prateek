namespace Mayfair.Core.Code.Service.Interfaces
{
    public interface IService<TProvider> : IService
        where TProvider : IServiceProvider
    {
        #region Service
        void Register(TProvider provider);
        void Unregister(TProvider provider);
        #endregion
    }
}

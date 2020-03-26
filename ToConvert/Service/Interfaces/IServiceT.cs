namespace Assets.Prateek.ToConvert.Service.Interfaces
{
    public interface IService<TProvider> : IService
        where TProvider : IServiceProvider
    {
        #region Service
        void Register(TProvider provider);
        #endregion

        #region Class Methods
        void Unregister(TProvider provider);
        #endregion
    }
}

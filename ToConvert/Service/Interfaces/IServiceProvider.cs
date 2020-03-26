namespace Assets.Prateek.ToConvert.Service.Interfaces
{
    using Assets.Prateek.ToConvert.Priority;

    public interface IServiceProvider : IPriority
    {
        #region Properties
        bool IsValid { get; }
        #endregion
    }
}

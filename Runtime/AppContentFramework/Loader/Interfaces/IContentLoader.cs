namespace Prateek.Runtime.AppContentFramework.Loader.Interfaces
{
    using Prateek.Runtime.AppContentFramework.Loader.Enums;

    public interface IContentLoader
    {
        #region Properties
        string Path { get; }
        bool HasFinishedLoading { get; }
        ContentAsyncStatus Status { get; }
        float PercentComplete { get; }
        #endregion
    }
}

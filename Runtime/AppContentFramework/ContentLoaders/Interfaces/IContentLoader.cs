namespace Prateek.Runtime.AppContentFramework.ContentLoaders.Interfaces
{
    using Prateek.Runtime.AppContentFramework.ContentLoaders.Enums;

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

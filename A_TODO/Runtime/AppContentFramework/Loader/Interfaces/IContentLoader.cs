namespace Prateek.A_TODO.Runtime.AppContentFramework.Loader.Interfaces
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;

    public interface IContentLoader
    {
        #region Properties
        string Location { get; }
        bool IsDone { get; }
        AsyncStatus Status { get; }
        float PercentComplete { get; }
        #endregion
    }
}

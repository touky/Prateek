namespace Mayfair.Core.Code.Resources.Loader
{
    using Mayfair.Core.Code.Resources.Enums;

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

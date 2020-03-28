namespace Mayfair.Core.Code.Service.Interfaces
{
    using Mayfair.Core.Code.Utils.Types.Priority;

    public interface IServiceProvider : IPriority
    {
        #region Properties
        bool IsValid { get; }
        #endregion
    }
}

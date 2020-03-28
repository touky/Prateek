namespace Mayfair.Core.Code.Database.Interfaces
{
    using Mayfair.Core.Code.Utils.Types.UniqueId;

    public interface IDatabaseEntry
    {
        #region Properties
        Keyname IdUnique { get; }
        #endregion
    }
}

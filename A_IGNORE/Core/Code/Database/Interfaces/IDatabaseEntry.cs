namespace Mayfair.Core.Code.Database.Interfaces
{
    using Prateek.KeynameFramework;

    public interface IDatabaseEntry
    {
        #region Properties
        Keyname IdUnique { get; }
        #endregion
    }
}

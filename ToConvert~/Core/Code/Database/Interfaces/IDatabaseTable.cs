namespace Mayfair.Core.Code.Database.Interfaces
{
    using System.Collections.Generic;

    public interface IDatabaseTable
    {
        #region Class Methods
        bool GetEntries(List<IDatabaseEntry> result);
        #endregion
    }
}

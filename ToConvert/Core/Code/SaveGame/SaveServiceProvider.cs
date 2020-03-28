namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service;

    public abstract class SaveServiceProvider : ServiceProviderBehaviour
    {
        #region Class Methods
        public abstract bool TrySave(IReadOnlyList<SaveDataIdentification> identifications);
        public abstract bool TryLoad(IReadOnlyList<SaveDataIdentification> identifications);
        #endregion
    }
}

namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class SaveServant : ServantTickable<SaveDaemon, SaveServant>
    {
        #region Class Methods
        public abstract bool TrySave(IReadOnlyList<SaveDataIdentification> identifications);
        public abstract bool TryLoad(IReadOnlyList<SaveDataIdentification> identifications);
        #endregion
    }
}

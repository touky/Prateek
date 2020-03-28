namespace Mayfair.Core.Code.SaveGame
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Branches;

    public abstract class SaveDaemonBranch : DaemonBranchBehaviour<SaveDaemonCore, SaveDaemonBranch>
    {
        #region Class Methods
        public abstract bool TrySave(IReadOnlyList<SaveDataIdentification> identifications);
        public abstract bool TryLoad(IReadOnlyList<SaveDataIdentification> identifications);
        #endregion
    }
}

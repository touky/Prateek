namespace Assets.Prateek.ToConvert.SaveGame
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Service;

    public abstract class SaveServiceProvider : ServiceProviderBehaviour
    {
        #region Class Methods
        public abstract bool TrySave(IReadOnlyList<SaveDataIdentification> identifications);
        public abstract bool TryLoad(IReadOnlyList<SaveDataIdentification> identifications);
        #endregion
    }
}

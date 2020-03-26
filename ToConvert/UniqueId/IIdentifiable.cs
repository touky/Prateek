namespace Assets.Prateek.ToConvert.UniqueId
{
    using System;

    public interface IIdentifiable : IEquatable<IIdentifiable>
    {
        #region Properties
        UniqueId UniqueId { get; }
        #endregion

        #region Class Methods
        void SetUniqueId(UniqueId uniqueId);
        #endregion
    }
}

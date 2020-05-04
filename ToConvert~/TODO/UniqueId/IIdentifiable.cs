namespace Mayfair.Core.Code.Utils.Types.UniqueId
{
    using System;

    public interface IIdentifiable : IEquatable<IIdentifiable>
    {
        #region Properties
        Keyname Keyname { get; }
        void SetUniqueId(Keyname keyname);
        #endregion
    }
}

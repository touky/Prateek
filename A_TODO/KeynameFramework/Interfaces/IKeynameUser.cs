namespace Mayfair.Core.Code.Utils.Types.UniqueId
{
    using System;

    public interface IKeynameUser : IEquatable<IKeynameUser>
    {
        #region Properties
        Keyname Keyname { get; }
        void SetUniqueId(Keyname keyname);
        #endregion
    }
}

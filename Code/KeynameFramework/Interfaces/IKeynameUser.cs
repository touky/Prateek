namespace Prateek.KeynameFramework.Interfaces
{
    using System;

    public interface IKeynameUser : IEquatable<IKeynameUser>
    {
        #region Properties
        Keyname Keyname { get; }
        void SetKeyname(Keyname keyname);
        #endregion
    }
}

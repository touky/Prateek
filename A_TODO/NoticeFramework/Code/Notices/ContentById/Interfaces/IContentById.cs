namespace Prateek.NoticeFramework.Notices
{
    using System.Collections.Generic;
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Enums;

    public interface IContentById
    {
        #region Properties
        KeynameMatchResult IdMatchRequirement { get; }
        List<Keyname> UniqueIds { get; }
        #endregion
    }
}

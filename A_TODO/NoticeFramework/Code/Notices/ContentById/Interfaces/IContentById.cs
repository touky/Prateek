namespace Prateek.NoticeFramework.Notices
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;

    public interface IContentById
    {
        #region Properties
        KeywordMatchResult IdMatchRequirement { get; }
        List<Keyname> UniqueIds { get; }
        #endregion
    }
}
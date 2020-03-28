namespace Mayfair.Core.Code.Messaging.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;

    public interface IContentById
    {
        #region Properties
        TagMatchResult IdMatchRequirement { get; }
        List<Keyname> UniqueIds { get; }
        #endregion
    }
}

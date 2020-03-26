namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.TagSystem;
    using Assets.Prateek.ToConvert.UniqueId;

    public interface IContentById
    {
        #region Properties
        TagMatchResult IdMatchRequirement { get; }
        List<UniqueId> UniqueIds { get; }
        #endregion
    }
}

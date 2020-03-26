namespace Assets.Prateek.ToConvert.Messaging.Messages
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.TagSystem;
    using Assets.Prateek.ToConvert.UniqueId;

    [DebuggerDisplay("{GetType().Name}, Sender: {sender.Owner.Name}")]
    public abstract class ContentByIdDirect : DirectMessage, IContentById
    {
        #region Fields
        private TagMatchResult idMatchRequirement = TagMatchResultType.Equal;
        private List<UniqueId> uniqueIds = new List<UniqueId>();
        #endregion

        #region Class Methods
        public virtual void Init(TagMatchResult idMatchRequirement, UniqueId uniqueId)
        {
            Init(idMatchRequirement);

            UniqueIds.Add(uniqueId);
        }

        public virtual void Init(TagMatchResult idMatchRequirement)
        {
            this.idMatchRequirement = idMatchRequirement;
        }

        public void CopyFrom(IContentById other)
        {
            idMatchRequirement = other.IdMatchRequirement;
            uniqueIds = other.UniqueIds;
        }
        #endregion

        #region IContentById Members
        public TagMatchResult IdMatchRequirement
        {
            get { return idMatchRequirement; }
        }

        public List<UniqueId> UniqueIds
        {
            get { return uniqueIds; }
        }
        #endregion
    }
}

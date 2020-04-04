namespace Prateek.NoticeFramework.Notices
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Prateek.NoticeFramework.Notices.Core;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class ContentByIdDirect : DirectNotice, IContentById
    {
        #region Fields
        private TagMatchResult idMatchRequirement = TagMatchResultType.Equal;
        private List<Keyname> uniqueIds = new List<Keyname>();
        #endregion

        #region Class Methods
        public virtual void Init(TagMatchResult idMatchRequirement, Keyname keyname)
        {
            Init(idMatchRequirement);

            UniqueIds.Add(keyname);
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

        public List<Keyname> UniqueIds
        {
            get { return uniqueIds; }
        }
        #endregion
    }
}

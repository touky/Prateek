namespace Prateek.NoticeFramework.Notices
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Enums;
    using Prateek.NoticeFramework.Notices.Core;

    [DebuggerDisplay("{GetType().Name}, Sender: {transmitter.Owner.Name}")]
    public abstract class ContentByIdRequest : RequestNotice, IContentById
    {
        #region Fields
        private KeynameMatchResult idMatchRequirement = KeynameMatchType.Equal;
        private List<Keyname> uniqueIds = new List<Keyname>();
        #endregion

        #region Class Methods
        public virtual void Init(KeynameMatchResult idMatchRequirement, Keyname keyname)
        {
            Init(idMatchRequirement);

            UniqueIds.Add(keyname);
        }

        public virtual void Init(KeynameMatchResult idMatchRequirement)
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
        public KeynameMatchResult IdMatchRequirement
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

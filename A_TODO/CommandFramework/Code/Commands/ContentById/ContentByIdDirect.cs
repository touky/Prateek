namespace Commands
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Commands.Core;
    using Prateek.KeynameFramework;
    using Prateek.KeynameFramework.Enums;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ContentByIdDirect : DirectCommand, IContentById
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

namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;

    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ContentByIdRequest : RequestCommand, IContentById
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

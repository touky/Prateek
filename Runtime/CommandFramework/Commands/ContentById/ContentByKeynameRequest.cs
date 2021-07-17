namespace Prateek.Runtime.CommandFramework.Commands.ContentById
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.Commands.ContentById.Interfaces;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Commands.Core.Commands;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;

    /// <summary>
    ///     Base class for any system that needs to receive request for keyname dependant datas
    /// </summary>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ContentByKeynameRequest
        : RequestCommand
        , IContentByKeynameRequest
    {
        #region Fields
        private KeynameMatchResult matchRequirement = KeynameMatchType.Equal;
        private List<Keyname> requestedKeynames = new List<Keyname>();
        #endregion

        #region Class Methods
        public virtual void Init(KeynameMatchResult matchRequirement, Keyname keyname)
        {
            Init(matchRequirement);

            RequestedKeynames.Add(keyname);
        }

        public virtual void Init(KeynameMatchResult matchRequirement)
        {
            this.matchRequirement = matchRequirement;
        }

        protected override bool ValidateResponse()
        {
            return holder.Validate<IContentByKeynameResponse>();
        }

        public void CopyFrom(IContentByKeynameRequest other)
        {
            matchRequirement = other.MatchRequirement;
            requestedKeynames.AddRange(other.RequestedKeynames);
        }
        #endregion

        #region IContentByKeynameRequest Members
        public KeynameMatchResult MatchRequirement { get { return matchRequirement; } }

        public List<Keyname> RequestedKeynames { get { return requestedKeynames; } }
        #endregion
    }
}

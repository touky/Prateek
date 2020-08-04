namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;

    /// <summary>
    /// Base class for any system that needs to receive request for keyname dependant datas
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TIdentification"></typeparam>
    [DebuggerDisplay("{GetType().Name}, Sender: {emitter.Owner.Name}")]
    public abstract class ContentByKeynameRequest<TResponse, TIdentification>
        : RequestCommand<TResponse, TIdentification>
        , IContentByKeynameRequest
        where TResponse : ResponseCommand, IContentByKeynameResponse, new()
        where TIdentification : Command
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

        public void CopyFrom(IContentByKeynameRequest other)
        {
            matchRequirement = other.MatchRequirement;
            requestedKeynames.AddRange(other.RequestedKeynames);
        }
        #endregion

        #region IContentById Members
        public KeynameMatchResult MatchRequirement
        {
            get { return matchRequirement; }
        }

        public List<Keyname> RequestedKeynames
        {
            get { return requestedKeynames; }
        }
        #endregion
    }
}

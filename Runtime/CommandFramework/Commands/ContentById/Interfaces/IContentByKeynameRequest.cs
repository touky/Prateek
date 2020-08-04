namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById.Interfaces
{
    using System.Collections.Generic;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;

    public interface IContentByKeynameRequest
    {
        #region Properties
        KeynameMatchResult MatchRequirement { get; }
        List<Keyname> RequestedKeynames { get; }
        #endregion
    }
}

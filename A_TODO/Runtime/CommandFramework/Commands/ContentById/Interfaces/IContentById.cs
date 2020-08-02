namespace Prateek.A_TODO.Runtime.CommandFramework.Commands.ContentById.Interfaces
{
    using System.Collections.Generic;
    using Prateek.Runtime.KeynameFramework;
    using Prateek.Runtime.KeynameFramework.Enums;

    public interface IContentById
    {
        #region Properties
        KeynameMatchResult IdMatchRequirement { get; }
        List<Keyname> UniqueIds { get; }
        #endregion
    }
}

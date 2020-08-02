namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;

    public class DatabaseIdentifierResponse : ResponseCommand
    {
        #region Properties
        public List<ICompositeIdentifier> Identifiers { get; } = new List<ICompositeIdentifier>();
        #endregion
    }
}
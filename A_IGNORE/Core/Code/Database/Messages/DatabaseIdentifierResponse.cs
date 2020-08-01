namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Interfaces;
    using Commands.Core;

    public class DatabaseIdentifierResponse : ResponseCommand
    {
        #region Properties
        public List<ICompositeIdentifier> Identifiers { get; } = new List<ICompositeIdentifier>();
        #endregion
    }
}
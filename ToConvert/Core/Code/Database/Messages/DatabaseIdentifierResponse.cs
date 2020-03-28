namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Interfaces;
    using Messaging.Messages;

    public class DatabaseIdentifierResponse : ResponseMessage
    {
        #region Properties
        public List<ICompositeIdentifier> Identifiers { get; } = new List<ICompositeIdentifier>();
        #endregion
    }
}
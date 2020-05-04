namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Interfaces;
    using Prateek.NoticeFramework.Notices.Core;

    public class DatabaseIdentifierResponse : ResponseNotice
    {
        #region Properties
        public List<ICompositeIdentifier> Identifiers { get; } = new List<ICompositeIdentifier>();
        #endregion
    }
}
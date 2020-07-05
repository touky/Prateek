namespace Mayfair.Core.Code.Database.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Database.Interfaces;
    using Prateek.NoticeFramework.Notices.Core;

    public class DatabaseIdentifierSetup : DirectNotice
    {
        #region Fields
        private List<ICompositeIdentifier> identifiers = new List<ICompositeIdentifier>();
        #endregion

        #region Properties
        public List<ICompositeIdentifier> Identifiers
        {
            get { return this.identifiers; }
        }
        #endregion

        #region Class Methods
        public void Add(ICompositeIdentifier identifier)
        {
            this.identifiers.Add(identifier);
        }
        #endregion
    }
}

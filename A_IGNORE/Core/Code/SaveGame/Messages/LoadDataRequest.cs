namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Collections.Generic;
    using Prateek.NoticeFramework.Notices.Core;

    public class LoadDataRequest : RequestNotice<LoadDataResponse>
    {
        #region Fields
        private List<SaveDataIdentification> identifications = new List<SaveDataIdentification>();
        #endregion

        #region Properties
        public IReadOnlyList<SaveDataIdentification> Identifications
        {
            get { return this.identifications; }
        }
        #endregion

        #region Class Methods
        public void Add(SaveDataIdentification identification)
        {
            this.identifications.Add(identification);
        }
        #endregion
    }
}

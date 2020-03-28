namespace Mayfair.Core.Code.SaveGame.Messages
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Messaging.Messages;

    public class LoadDataRequest : RequestMessage<LoadDataResponse>
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

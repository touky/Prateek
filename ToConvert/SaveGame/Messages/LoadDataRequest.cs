namespace Assets.Prateek.ToConvert.SaveGame.Messages
{
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public class LoadDataRequest : RequestMessage<LoadDataResponse>
    {
        #region Fields
        private List<SaveDataIdentification> identifications = new List<SaveDataIdentification>();
        #endregion

        #region Properties
        public IReadOnlyList<SaveDataIdentification> Identifications
        {
            get { return identifications; }
        }
        #endregion

        #region Class Methods
        public void Add(SaveDataIdentification identification)
        {
            identifications.Add(identification);
        }
        #endregion
    }
}

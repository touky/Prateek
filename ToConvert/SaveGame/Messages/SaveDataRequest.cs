namespace Assets.Prateek.ToConvert.SaveGame.Messages
{
    using Assets.Prateek.ToConvert.Messaging.Messages;

    public class SaveDataRequest : DirectMessage
    {
        #region StatusType enum
        public enum StatusType
        {
            Preparing,
            Valid
        }
        #endregion

        #region Fields
        private StatusType status;
        #endregion

        #region Properties
        public StatusType Status
        {
            get { return status; }
        }
        #endregion

        #region Constructors
        public SaveDataRequest(StatusType status)
        {
            this.status = status;
        }
        #endregion
    }
}

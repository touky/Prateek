namespace Mayfair.Core.Code.SaveGame.Messages
{
    using Prateek.Runtime.CommandFramework.Commands.Core;

    public class SaveDataRequest : DirectCommand
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
            get { return this.status; }
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

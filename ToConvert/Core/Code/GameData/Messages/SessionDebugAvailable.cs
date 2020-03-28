namespace Mayfair.Core.Code.GameData.Messages
{
    using Mayfair.Core.Code.Messaging.Messages;

    public class SessionDebugAvailable : DirectMessage
    {
        #region Fields
        private string sessionContext;
        #endregion

        #region Properties
        public string SessionContext
        {
            get { return this.sessionContext; }
        }
        #endregion

        #region Class Methods
        public void Init(string sessionContext)
        {
            this.sessionContext = sessionContext;
        }
        #endregion
    }
}

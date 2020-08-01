namespace Mayfair.Core.Code.GameData.Messages
{
    using Commands.Core;

    public class SessionDebugAvailable : DirectCommand
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

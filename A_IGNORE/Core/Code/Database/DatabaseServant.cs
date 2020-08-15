namespace Mayfair.Core.Code.Database
{
    using Prateek.Runtime.AppContentFramework.Daemons;

    public abstract class DatabaseServant : ContentAccessServant<DatabaseDaemonOverseer, DatabaseServant>
    {
        #region Static and Constants
        public static readonly string[] KEYWORDS = { "Database/" };
        #endregion

        #region Properties
        public string[] ResourceKeywords
        {
            get { return KEYWORDS; }
        }
        #endregion

        #region Class Methods
        public abstract void RefreshPendingResources(DatabaseDaemonOverseer daemonOverseerCore);
        #endregion
    }
}

namespace Mayfair.Core.Code.Database
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Daemons;

    public abstract class DatabaseServant : ContentAccessServant<DatabaseDaemon, DatabaseServant>
    {
        #region Static and Constants
        public static readonly string[] KEYWORDS = { "Database/" };
        #endregion

        #region Properties
        public override string[] ResourceKeywords
        {
            get { return KEYWORDS; }
        }
        #endregion

        #region Class Methods
        public abstract void RefreshPendingResources(DatabaseDaemon daemonCore);
        #endregion
    }
}

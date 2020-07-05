namespace Mayfair.Core.Code.Database
{
    using Mayfair.Core.Code.Resources;

    public abstract class DatabaseDaemonBranch : ContentAccessDaemonBranch<DatabaseDaemonCore, DatabaseDaemonBranch>
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
        public abstract void RefreshPendingResources(DatabaseDaemonCore daemonCore);
        #endregion
    }
}

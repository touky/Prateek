namespace Mayfair.Core.Code.Database
{
    using Mayfair.Core.Code.Resources;

    public abstract class DatabaseServiceProvider : ResourceDependentServiceProvider<DatabaseService, DatabaseServiceProvider>
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
        protected override void OnIdentificationRequested()
        {
            SendIdentificationFor<DatabaseService, DatabaseServiceProvider>(this);
        }

        public abstract void RefreshPendingResources(DatabaseService service);
        #endregion
    }
}

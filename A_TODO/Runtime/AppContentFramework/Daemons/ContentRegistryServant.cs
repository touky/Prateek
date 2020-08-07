namespace Prateek.A_TODO.Runtime.AppContentFramework.Daemons
{
    using Prateek.A_TODO.Runtime.AppContentFramework.Enums;
    using Prateek.Runtime.DaemonFramework.Servants;

    public abstract class ContentRegistryServant
        : Servant<ContentRegistryDaemon, ContentRegistryServant>
    {
        #region Class Methods
        public abstract void ExecuteState(ContentRegistryDaemon daemonCore, ServiceState state);
        #endregion
    }
}

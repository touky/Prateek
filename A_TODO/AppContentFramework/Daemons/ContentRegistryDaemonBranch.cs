namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Resources.Enums;
    using Prateek.DaemonFramework.Code.Servants;

    public abstract class ContentRegistryServant
        : ServantBehaviour<ContentRegistryDaemon, ContentRegistryServant>
    {
        #region Class Methods
        public abstract void ExecuteState(ContentRegistryDaemon daemonCore, ServiceState state);
        #endregion
    }
}

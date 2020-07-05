namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Resources.Enums;
    using Prateek.DaemonFramework.Code.Branches;

    public abstract class ContentRegistryDaemonBranch
        : DaemonBranchBehaviour<ContentRegistryDaemonCore, ContentRegistryDaemonBranch>
    {
        #region Class Methods
        public abstract void ExecuteState(ContentRegistryDaemonCore daemonCore, ServiceState state);
        #endregion
    }
}

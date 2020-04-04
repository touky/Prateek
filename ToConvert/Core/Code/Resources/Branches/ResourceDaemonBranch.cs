namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Resources.Enums;
    using Prateek.DaemonCore.Code.Branches;

    public abstract class ResourceDaemonBranch : DaemonBranchBehaviour<ResourceDaemonCore, ResourceDaemonBranch>
    {
        #region Class Methods
        public abstract void ExecuteState(ResourceDaemonCore daemonCore, ServiceState state);
        #endregion
    }
}

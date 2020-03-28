namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Resources.Messages;
    using Mayfair.Core.Code.Service;
    using Prateek.DaemonCore.Code.Branches;

    public abstract class ResourceDependentDaemonBranch<TDaemonCore, TDaemonBranch> : DaemonBranchBehaviour<TDaemonCore, TDaemonBranch>
        where TDaemonCore : DaemonCoreCommunicator<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : ResourceDependentDaemonBranch<TDaemonCore, TDaemonBranch>
    {
        #region Properties
        public abstract string[] ResourceKeywords { get; }
        #endregion

        #region Class Methods
        public abstract RequestCallbackOnChange GetResourceChangeRequest(ILightMessageCommunicator communicator);
        public abstract void OnResourceChanged(TDaemonCore service, ResourcesHaveChangedResponse message);
        #endregion
    }
}

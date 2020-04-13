namespace Mayfair.Core.Code.Resources
{
    using Mayfair.Core.Code.Resources.Messages;
    using Prateek.DaemonCore.Code.Branches;
    using Prateek.NoticeFramework.Tools;
    using Prateek.NoticeFramework.TransmitterReceiver;

    public abstract class ContentAccessDaemonBranch<TDaemonCore, TDaemonBranch>
        : DaemonBranchBehaviour<TDaemonCore, TDaemonBranch>
        where TDaemonCore : NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : ContentAccessDaemonBranch<TDaemonCore, TDaemonBranch>
    {
        #region Properties
        public abstract string[] ResourceKeywords { get; }
        #endregion

        #region Class Methods
        public abstract RequestAccessToContent GetResourceChangeRequest(INoticeTransmitter transmitter);
        public abstract void OnResourceChanged(TDaemonCore service, ResourcesHaveChangedResponse notice);
        #endregion
    }
}

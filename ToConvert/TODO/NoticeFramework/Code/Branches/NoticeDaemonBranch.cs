namespace Prateek.NoticeFramework.Branches
{
    using Prateek.DaemonFramework.Code.Branches;
    using Prateek.NoticeFramework.Notices.Core;

    public abstract class NoticeDaemonBranch : DaemonBranchBehaviour<NoticeDaemonCore, NoticeDaemonBranch>
    {
        #region Class Methods
        public abstract void ReceiveNotice(NoticeDaemonCore daemonCore, Notice receivedNotice);
        #endregion
    }
}

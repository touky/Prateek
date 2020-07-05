namespace Prateek.NoticeFramework.Branches
{
    using Prateek.NoticeFramework.Notices.Core;

    public sealed class LocalNoticeDaemonBranch : NoticeDaemonBranch
    {
        #region Class Methods
        public override void ReceiveNotice(NoticeDaemonCore daemonCore, Notice receivedNotice)
        {
            daemonCore.AddMessage(receivedNotice);
        }
        #endregion
    }
}

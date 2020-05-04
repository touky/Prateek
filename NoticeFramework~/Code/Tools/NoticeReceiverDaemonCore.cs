namespace Prateek.NoticeFramework.Tools
{
    using Prateek.DaemonCore.Code;
    using Prateek.DaemonCore.Code.Interfaces;
    using Prateek.NoticeFramework.TransmitterReceiver;
    using UnityEngine;

    public abstract class NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        : DaemonCore<TDaemonCore, TDaemonBranch>, INoticeReceiverOwner
        where TDaemonCore : NoticeReceiverDaemonCore<TDaemonCore, TDaemonBranch>
        where TDaemonBranch : class, IDaemonBranch
    {
        #region Fields
        private INoticeReceiver noticeReceiver;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            InitNoticeReceiver();

            base.Awake();
        }

        protected virtual void Update()
        {
            UpdateNoticeReceiver();
        }

        protected virtual void OnDestroy()
        {
            this.noticeReceiver.CleanUp();
        }
        #endregion

        #region Class Methods
        protected void InitNoticeReceiver()
        {
            this.noticeReceiver = NoticeDaemonCore.CreateNoticeReceiver(this);
            SetupNoticeReceiverCallback();
            this.noticeReceiver.ApplyCallbacks();
        }

        protected void UpdateNoticeReceiver()
        {
            if (NoticeReceiver.HasNotice())
            {
                NoticeReceiver.ProcessAllNotices();
            }
        }

        protected abstract void SetupNoticeReceiverCallback();
        #endregion

        #region IMessageCommunicatorOwner Members
        public INoticeReceiver NoticeReceiver
        {
            get { return this.noticeReceiver; }
        }

        public string Name
        {
            get { return name; }
        }

        public Transform Transform
        {
            get { return transform; }
        }

        public virtual void NoticeReceived() { }
        #endregion
    }
}

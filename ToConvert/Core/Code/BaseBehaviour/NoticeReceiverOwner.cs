namespace Mayfair.Core.Code.BaseBehaviour
{
    using Prateek.NoticeFramework;
    using Prateek.NoticeFramework.TransmitterReceiver;
    using UnityEngine;

    public abstract class NoticeReceiverOwner : MonoBehaviour, INoticeReceiverOwner
    {
        #region Fields
        private INoticeReceiver noticeReceiver;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            InitNoticeReceiver();
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

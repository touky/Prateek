namespace Mayfair.Core.Code.BaseBehaviour
{
    using Prateek.NoticeFramework;
    using Prateek.NoticeFramework.TransmitterReceiver;
    using UnityEngine;

    public abstract class AutoDisableNoticeReceiverOwnerBehaviour : AutoDisableBehaviour, INoticeReceiverOwner
    {
        #region Fields
        private INoticeReceiver noticeReceiver;
        #endregion

        #region Unity Methods
        protected override void Awake()
        {
            base.Awake();

            InitNoticeReceiver();
        }

        protected void Update()
        {
            UpdateNoticeReceiver();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

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

        public void NoticeReceived()
        {
            WakeUp();
        }
        #endregion
    }
}

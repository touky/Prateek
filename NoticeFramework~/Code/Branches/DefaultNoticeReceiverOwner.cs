namespace Prateek.NoticeFramework.Branches {
    using System;
    using Prateek.NoticeFramework.TransmitterReceiver;
    using UnityEngine;

    internal class DefaultNoticeReceiverOwner : INoticeReceiverOwner
    {
        #region Fields
        private INoticeReceiver noticeReceiver;
        #endregion

        #region Constructors
        public DefaultNoticeReceiverOwner()
        {
            noticeReceiver = NoticeDaemonCore.CreateNoticeReceiver(this);
        }
        #endregion

        #region IMessageCommunicatorOwner Members
        public INoticeReceiver NoticeReceiver
        {
            get { return noticeReceiver; }
        }

        public string Name
        {
            get { return GetType().Name; }
        }

        public Transform Transform
        {
            get { return null; }
        }

        public void NoticeReceived()
        {
            throw new NotImplementedException($"Cannot receiver notices through the {typeof(DefaultNoticeReceiverOwner).Name}");
        }
        #endregion
    }
}
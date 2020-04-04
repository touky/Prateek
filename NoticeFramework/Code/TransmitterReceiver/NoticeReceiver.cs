namespace Prateek.NoticeFramework.TransmitterReceiver {
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.DaemonCore.Code;
    using Prateek.NoticeFramework.Branches;
    using Prateek.NoticeFramework.Notices.Core;

    [DebuggerDisplay("Owner: {owner.Name}")]
    internal sealed class NoticeReceiver : INoticeReceiver
    {
        #region Fields
        private INoticeReceiverOwner owner;
        private bool noticeReadLock = false;
        private List<Notice> receivedNotices = new List<Notice>();
        private List<Notice> processingNotices = new List<Notice>();
        private Dictionary<long, INoticeCallbackProxy> noticeCallbacks = new Dictionary<long, INoticeCallbackProxy>();

        private List<NoticeId> callbackToRegister = new List<NoticeId>();
        private List<NoticeId> callbackToUnregister = new List<NoticeId>();
        #endregion

        #region Properties
        public List<NoticeId> CallbackToRegister
        {
            get { return callbackToRegister; }
        }

        public List<NoticeId> CallbackToUnregister
        {
            get { return callbackToUnregister; }
        }
        #endregion

        #region Constructors
        public NoticeReceiver(INoticeReceiverOwner owner)
        {
            this.owner = owner;
        }
        #endregion

        #region Class Methods
        public void Receive(Notice receivedNotice)
        {
            //Disallow any noticeReceiver from receiving their notices now
            //MessageReceived() callback is only meant for async purpose
            if (receivedNotices.Count == 0)
            {
                noticeReadLock = true;
                {
                    owner.NoticeReceived();
                }
                noticeReadLock = false;
            }

            receivedNotices.Add(receivedNotice);
        }
        #endregion

        #region IMessageCommunicator Members
        public void CleanUp()
        {
            if (SingletonBehaviour<NoticeDaemonCore>.ApplicationIsQuitting)
            {
                return;
            }

            ClearCallbacks();
            ApplyCallbacks();
        }

        public INoticeReceiverOwner Owner
        {
            get { return owner; }
        }
        #endregion

        #region Sending
        public void Broadcast(BroadcastNotice notice)
        {
            Send(notice);
        }

        public void Send(TargetedNotice notice)
        {
            Send((Notice) notice);
        }

        public void Send(DirectNotice notice)
        {
            Send((Notice) notice);
        }

        public void Send(ResponseNotice notice)
        {
            UnityEngine.Debug.Assert(notice != null, "notice != null");
            UnityEngine.Debug.Assert(notice.Recipient != null, $"notice.Recipient != null, Type: {notice.GetType().Name}");

            Send((Notice) notice);
        }

        private void Send(Notice notice)
        {
            UnityEngine.Debug.Assert(notice != null);

            notice.Transmitter = this;

            NoticeDaemonCore.ReceiveMessage(notice);
        }
        #endregion

        #region Receiving
        public bool HasNotice()
        {
            return receivedNotices.Count > 0;
        }

        public void ProcessAllNotices()
        {
            if (noticeReadLock)
            {
                UnityEngine.Debug.Assert(false, "ProcessAllMessages() call is forbidden on MessageReceived()");
                return;
            }

            processingNotices.AddRange(receivedNotices);
            receivedNotices.Clear();

            INoticeCallbackProxy callback = null;
            foreach (Notice notice in processingNotices)
            {
                long noticeId = notice.NoticeID;
                if (noticeCallbacks.TryGetValue(noticeId, out callback))
                {
                    callback.Invoke(notice);
                }
                else
                {
                    //This can happen, if no noticeReceiver has been registered to this type of event
                    //We log a warning for now (18/10/19) since this is not supposed to be a problem in this system logic
                    //todo DebugTools.LogWarning($"No recipient found for notice {notice.GetType().Name} sent by {notice.Sender.Owner.Name}");
                }
            }

            processingNotices.Clear();
        }
        #endregion

        #region Callback management
        public void AddCallback<T>(NoticeCallback<T> noticeCallback) where T : Notice
        {
            NoticeId register = new NoticeId(typeof(T), this);

            InternalAddCallback(register, new NoticeCallbackProxy<T>(noticeCallback));

            SetCallbackAsPending(register, true);
        }

        public void RemoveCallback<T>() where T : Notice
        {
            NoticeId id = new NoticeId(typeof(T), this);

            SetCallbackAsPending(id, false);
        }

        public void ClearCallbacks()
        {
            foreach (long key in noticeCallbacks.Keys)
            {
                SetCallbackAsPending(key, false);
            }
        }

        public void ApplyCallbacks()
        {
            NoticeDaemonCore.RefreshRegistration(this);

            CallbackToRegister.Clear();
            CallbackToUnregister.Clear();
        }

        private void InternalAddCallback(NoticeId register, INoticeCallbackProxy callbackProxy)
        {
            long id = register.GetValidId();

            if (!noticeCallbacks.ContainsKey(id))
            {
                noticeCallbacks.Add(id, callbackProxy);
            }
            else
            {
                noticeCallbacks[id] = callbackProxy;
            }
        }

        private void SetCallbackAsPending(NoticeId id, bool doRegister)
        {
            List<NoticeId> removeList = doRegister ? callbackToUnregister : callbackToRegister;
            List<NoticeId> addList    = doRegister ? callbackToRegister : callbackToUnregister;

            if (removeList.Contains(id))
            {
                removeList.Remove(id);
            }

            if (!addList.Contains(id))
            {
                addList.Add(id);
            }
        }
        #endregion
    }
}
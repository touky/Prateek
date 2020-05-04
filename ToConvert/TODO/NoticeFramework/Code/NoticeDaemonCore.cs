namespace Prateek.NoticeFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Prateek.DaemonFramework.Code;
    using Prateek.NoticeFramework.Branches;
    using Prateek.NoticeFramework.Debug;
    using Prateek.NoticeFramework.Notices.Core;
    using Prateek.NoticeFramework.TransmitterReceiver;
    using Prateek.TickableFramework.Code.Enums;

    #region Nested type: MessageService
    public sealed partial class NoticeDaemonCore : DaemonCore<NoticeDaemonCore, NoticeDaemonBranch>, IDebugMenuNotebookOwner
    {
        #region Fields
        private Dictionary<long, HashSet<NoticeReceiver>> liveClients;
        private DefaultNoticeReceiverOwner defaultReceiverOwner;
        private readonly object noticeLock = new object();
        private List<Notice> receivedNotices;
        private List<Notice> processedNotices;

        private LiveNoticeReceiversMenuPage debugLivePage = null;
        #endregion

        #region Properties
        public static INoticeTransmitter DefaultNoticeTransmitter
        {
            get { return Instance.defaultReceiverOwner.NoticeReceiver; }
        }
        #endregion
        
        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }

        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            ProcessNotices();
        }

        #region Service
        protected override void OnAwake()
        {
            if (liveClients == null)
            {
                liveClients = new Dictionary<long, HashSet<NoticeReceiver>>();
            }

            processedNotices = new List<Notice>();

            lock (noticeLock)
            {
                receivedNotices = new List<Notice>();
            }

            SetupDebugContent();

            defaultReceiverOwner = new DefaultNoticeReceiverOwner();
        }

        internal void Register(NoticeReceiver noticeReceiver)
        {
            foreach (NoticeId register in noticeReceiver.CallbackToRegister)
            {
                long noticeId = register.GetValidId();
                if (liveClients.TryGetValue(noticeId, out HashSet<NoticeReceiver> receivers))
                {
                    if (register.Type.IsSubclassOf(typeof(TargetedNotice)))
                    {
                        if (receivers.Count > 0 && !receivers.Contains(noticeReceiver))
                        {
                            //This should not happen
                            System.Diagnostics.Debug.Assert(false, $"{noticeId}: Another noticeReceiver is already registered on this notice !!!!");

                            continue;
                        }
                    }

                    receivers.Add(noticeReceiver);
                }
                else
                {
                    receivers = new HashSet<NoticeReceiver>();
                    receivers.Add(noticeReceiver);
                    liveClients.Add(register.GetValidId(), receivers);

                    AddType(register.Type);
                }
            }
        }

        internal static void RefreshRegistration(NoticeReceiver noticeReceiver)
        {
            Instance.Unregister(noticeReceiver);
            Instance.Register(noticeReceiver);
        }

        internal void Unregister(NoticeReceiver noticeReceiver)
        {
            foreach (NoticeId register in noticeReceiver.CallbackToUnregister)
            {
                if (liveClients.TryGetValue(register.GetValidId(), out HashSet<NoticeReceiver> receivers))
                {
                    receivers.Remove(noticeReceiver);
                }
            }
        }
        #endregion

        #region Class Methods
        public static INoticeReceiver CreateNoticeReceiver(INoticeReceiverOwner owner)
        {
            return new NoticeReceiver(owner);
        }

        internal static void ReceiveMessage(Notice receivedNotice)
        {
            NoticeDaemonBranch branch = Instance.GetFirstAliveBranch();
            if (branch == null)
            {
                return;
            }

            branch.ReceiveNotice(Instance, receivedNotice);
        }

        public void AddMessage(Notice receivedNotice)
        {
            lock (noticeLock)
            {
                receivedNotices.Add(receivedNotice);
            }
        }

        private void ProcessNotices()
        {
            //Emptying the received notices to ensure that no notice received in-between processing interferes with the processing
            lock (noticeLock)
            {
                if (receivedNotices.Count == 0)
                {
                    return;
                }

                processedNotices.AddRange(receivedNotices);
                receivedNotices.Clear();
            }

            StringBuilder builder = null;

            HashSet<NoticeReceiver> receivers = null;
            foreach (Notice notice in processedNotices)
            {
                //todo builder.AddReceivedMessage(this, notice);

                //Standard notice management, send to concerned receivers
                long noticeId = notice.NoticeID;
                if (liveClients.TryGetValue(noticeId, out receivers))
                {
                    foreach (NoticeReceiver noticeReceiver in receivers)
                    {
                        //todo builder.AddCommunicator(this, noticeReceiver);

                        noticeReceiver.Receive(notice);

                        //if (connectionDrawer == null)
                        //{
                        //    connectionDrawer = new GameObject().AddComponent<MessageServiceDrawer>();
                        //}
                        //else
                        //{
                        //    connectionDrawer.Tag(noticeReceiver, notice);
                        //}
                    }
                }
            }

            //todo DebugTools.Log(builder, DebugTools.LogLevel.Verbose);

            processedNotices.Clear();
        }
        #endregion

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        private void SetupDebugContent()
        {
            //DebugMenuNotebook debugNotebook = new DebugMenuNotebook("MSGS", "Message Service");
            //debugLivePage = new LiveNoticeReceiversMenuPage(this, "Live receivers");

            //debugNotebook.AddPagesWithParent(new EmptyMenuPage("MAIN"), debugLivePage);
            //debugNotebook.Register();
        }

        [Conditional("NVIZZIO_DEV")]
        private void AddType(Type type)
        {
            if (debugLivePage == null)
            {
                return;
            }

            debugLivePage.AddType(type);
        }
        #endregion
    }
    #endregion
}

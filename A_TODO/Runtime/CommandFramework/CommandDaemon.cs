namespace Prateek.A_TODO.Runtime.CommandFramework
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Mayfair.Core.Code.DebugMenu.Content;
    using Prateek.A_TODO.Runtime.CommandFramework.Commands.Core;
    using Prateek.A_TODO.Runtime.CommandFramework.Debug;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver;
    using Prateek.A_TODO.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.A_TODO.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.TickableFramework.Enums;

    #region Nested type: MessageService
    public sealed partial class CommandDaemon : DaemonOverseer<CommandDaemon, CommandServant>, IDebugMenuNotebookOwner
    {
        #region Fields
        private Dictionary<long, HashSet<CommandReceiver>> liveClients;
        private DefaultCommandReceiverOwner defaultReceiverOwner;
        private readonly object noticeLock = new object();
        private List<Command> receivedCommands;
        private List<Command> processingCommands;

        private LiveNoticeReceiversMenuPage debugLivePage = null;
        #endregion

        #region Properties
        public static ICommandEmitter DefaultCommandEmitter
        {
            get { return Instance.defaultReceiverOwner.CommandReceiver; }
        }

        public override TickableSetup TickableSetup
        {
            get { return TickableSetup.UpdateBegin; }
        }
        #endregion

        #region Class Methods
        public override void Tick(TickableFrame tickableFrame, float seconds, float unscaledSeconds)
        {
            base.Tick(tickableFrame, seconds, unscaledSeconds);

            ProcessNotices();
        }

        public static ICommandReceiver CreateCommandReceiver(ICommandReceiverOwner owner)
        {
            return new CommandReceiver(owner);
        }

        internal static void ReceiveMessage(Command receivedCommand)
        {
            var servant = Instance.FirstAliveServant;
            if (servant == null)
            {
                return;
            }

            servant.ReceiveNotice(Instance, receivedCommand);
        }

        public void AddMessage(Command receivedCommand)
        {
            lock (noticeLock)
            {
                receivedCommands.Add(receivedCommand);
            }
        }

        private void ProcessNotices()
        {
            //Emptying the received notices to ensure that no notice received in-between processing interferes with the processing
            lock (noticeLock)
            {
                if (receivedCommands.Count == 0)
                {
                    return;
                }

                processingCommands.AddRange(receivedCommands);
                receivedCommands.Clear();
            }

            var builder = (StringBuilder)null;
            var receivers = (HashSet<CommandReceiver>)null;

            foreach (var notice in processingCommands)
            {
                //todo builder.AddReceivedMessage(this, notice);

                //Standard notice management, send to concerned receivers
                var commandId = notice.CommandID;
                if (liveClients.TryGetValue(commandId, out receivers))
                {
                    foreach (var commandReceiver in receivers)
                    {
                        //todo builder.AddCommunicator(this, noticeReceiver);

                        commandReceiver.Receive(notice);

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

            processingCommands.Clear();
        }

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

        #region Service
        protected override void OnAwake()
        {
            if (liveClients == null)
            {
                liveClients = new Dictionary<long, HashSet<CommandReceiver>>();
            }

            processingCommands = new List<Command>();

            lock (noticeLock)
            {
                receivedCommands = new List<Command>();
            }

            SetupDebugContent();

            defaultReceiverOwner = new DefaultCommandReceiverOwner();
        }

        internal void Register(CommandReceiver commandReceiver)
        {
            foreach (var register in commandReceiver.CallbackToRegister)
            {
                var noticeId = register.GetValidId();
                if (liveClients.TryGetValue(noticeId, out var receivers))
                {
                    if (register.Type.IsSubclassOf(typeof(TargetedCommand)))
                    {
                        if (receivers.Count > 0 && !receivers.Contains(commandReceiver))
                        {
                            //This should not happen
                            System.Diagnostics.Debug.Assert(false, $"{noticeId}: Another noticeReceiver is already registered on this notice !!!!");

                            continue;
                        }
                    }

                    receivers.Add(commandReceiver);
                }
                else
                {
                    receivers = new HashSet<CommandReceiver>();
                    receivers.Add(commandReceiver);
                    liveClients.Add(register.GetValidId(), receivers);

                    AddType(register.Type);
                }
            }
        }

        internal static void RefreshRegistration(CommandReceiver commandReceiver)
        {
            Instance.Unregister(commandReceiver);
            Instance.Register(commandReceiver);
        }

        internal void Unregister(CommandReceiver commandReceiver)
        {
            foreach (var register in commandReceiver.CallbackToUnregister)
            {
                if (liveClients.TryGetValue(register.GetValidId(), out var receivers))
                {
                    receivers.Remove(commandReceiver);
                }
            }
        }
        #endregion
    }
    #endregion
}

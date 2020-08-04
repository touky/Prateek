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
    public sealed class CommandDaemon
        : DaemonOverseer<CommandDaemon, CommandServant>
        , IDebugMenuNotebookOwner
    {
        #region Fields
        private readonly object noticeLock = new object();
        private Dictionary<long, HashSet<CommandReceiver>> liveReceivers;

        private DefaultCommandEmitter defaultEmitter;
        
        private List<Command> commandReceived;
        private List<Command> commandCached;

        private LiveNoticeReceiversMenuPage debugLivePage = null;
        #endregion

        #region Properties
        public static ICommandEmitter DefaultEmitter
        {
            get { return Instance.defaultEmitter.CommandReceiver; }
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

        internal static void CommandReceived(Command receivedCommand)
        {
            foreach (var servant in Instance.AllAliveServants)
            {
                servant.CommandReceived(receivedCommand);
            }
        }

        internal void FlushCommand(Command receivedCommand)
        {
            commandReceived.Add(receivedCommand);
        }

        private void ProcessNotices()
        {
            lock (noticeLock)
            {
                foreach (var servant in Instance.AllAliveServants)
                {
                    servant.FlushReceivedCommands();
                }
            }

            //Emptying the received notices to ensure that no notice received in-between processing interferes with the processing
            lock (noticeLock)
            {
                if (commandReceived.Count == 0)
                {
                    return;
                }

                commandCached.AddRange(commandReceived);
                commandReceived.Clear();
            }

            //todo var builder = (StringBuilder)null;
            foreach (var notice in commandCached)
            {
                //todo builder.AddReceivedMessage(this, notice);

                //Standard notice management, send to concerned receivers
                var commandId = notice.CommandId;
                if (liveReceivers.TryGetValue(commandId.Key, out var receivers))
                {
                    foreach (var receiver in receivers)
                    {
                        //todo builder.AddCommunicator(this, noticeReceiver);

                        receiver.Receive(notice);
                    }
                }
            }

            //todo DebugTools.Log(builder, DebugTools.LogLevel.Verbose);

            commandCached.Clear();
        }

        [Conditional("PRATEEK_DEBUG")]
        private void SetupDebugContent()
        {
            //DebugMenuNotebook debugNotebook = new DebugMenuNotebook("MSGS", "Message Service");
            //debugLivePage = new LiveNoticeReceiversMenuPage(this, "Live receivers");

            //debugNotebook.AddPagesWithParent(new EmptyMenuPage("MAIN"), debugLivePage);
            //debugNotebook.Register();
        }

        [Conditional("PRATEEK_DEBUG")]
        private void AddTypeToDebug(Type type)
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
            if (liveReceivers == null)
            {
                liveReceivers = new Dictionary<long, HashSet<CommandReceiver>>();
            }

            commandCached = new List<Command>();

            lock (noticeLock)
            {
                commandReceived = new List<Command>();
            }

            SetupDebugContent();

            defaultEmitter = new DefaultCommandEmitter();
        }

        internal static void FlushPendingActions(CommandReceiver commandReceiver)
        {
            Instance.Unregister(commandReceiver);
            Instance.Register(commandReceiver);
        }

        private void Register(CommandReceiver commandReceiver)
        {
            foreach (var commandId in commandReceiver.ActionsToRegister)
            {
                var keyId = commandId.Key;
                if (!liveReceivers.TryGetValue(keyId, out var receivers))
                {
                    receivers = new HashSet<CommandReceiver>();
                    liveReceivers.Add(keyId, receivers);
                }
                
                if (commandId.Type.IsSubclassOf(typeof(TargetedCommand)))
                {
                    if (receivers.Count > 0 && !receivers.Contains(commandReceiver))
                    {
                        //This should not happen
                        System.Diagnostics.Debug.Assert(false, $"{keyId}: Another noticeReceiver is already registered on this notice !!!!");
                    }
                }

                receivers.Add(commandReceiver);

                AddTypeToDebug(commandId.Type);
            }
        }

        private void Unregister(CommandReceiver commandReceiver)
        {
            foreach (var commandId in commandReceiver.ActionsToUnregister)
            {
                if (liveReceivers.TryGetValue(commandId.Key, out var receivers))
                {
                    receivers.Remove(commandReceiver);
                }
            }
        }
        #endregion
    }
    #endregion
}

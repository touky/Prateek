namespace Prateek.Runtime.CommandFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Commands.Core.Commands;
    using Prateek.Runtime.CommandFramework.Debug;
    using Prateek.Runtime.CommandFramework.Gadgets;
    using Prateek.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.Core.Interfaces.IPriority;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    #region Nested type: MessageService
    public sealed class CommandDaemon
        : DaemonOverseer<CommandDaemon, CommandServant>
        , DebugMenu.IDocumentOwner
        , IEarlyUpdateTickable
    {
        #region Fields
        private readonly object noticeLock = new object();
        private Dictionary<long, HashSet<Receiver>> liveReceivers;

        private DefaultCommandEmitter defaultEmitter;

        private List<Command> commandReceived;
        private List<Command> commandCached;
        #endregion

        #region Properties
        public static CommandTools.IEmitter DefaultEmitter
        {
            get { return Instance.defaultEmitter.Emitter; }
        }
        #endregion

        #region Class Methods
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

        private void ProcessReceivedCommands()
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
            foreach (var command in commandCached)
            {
                //todo builder.AddReceivedMessage(this, notice);

                //Standard notice management, send to concerned receivers
                var commandId = command.CommandId;
                if (liveReceivers.TryGetValue(commandId.Key, out var receivers))
                {
                    foreach (var receiver in receivers)
                    {
                        //todo builder.AddCommunicator(this, noticeReceiver);

                        receiver.Receive(command);
                    }
                }
            }

            //todo DebugTools.Log(builder, DebugTools.LogLevel.Verbose);

            commandCached.Clear();
        }
        #endregion

        #region IEarlyUpdateTickable Members
        public void EarlyUpdate()
        {
            ProcessReceivedCommands();
        }

        public int Priority(IPriority<IEarlyUpdateTickable> type)
        {
            return DefaultPriority;
        }
        #endregion

        #region Service
        protected override void OnAwake()
        {
            base.OnAwake();

            if (liveReceivers == null)
            {
                liveReceivers = new Dictionary<long, HashSet<Receiver>>();
            }

            commandCached = new List<Command>();

            lock (noticeLock)
            {
                commandReceived = new List<Command>();
            }

            defaultEmitter = new DefaultCommandEmitter();
        }

        internal static void FlushPendingActions(Receiver receiver)
        {
            Instance.Unregister(receiver);
            Instance.Register(receiver);
        }

        private void Register(Receiver receiver)
        {
            foreach (var commandId in receiver.ActionsToRegister)
            {
                var keyId = commandId.Key;
                if (!liveReceivers.TryGetValue(keyId, out var receivers))
                {
                    receivers = new HashSet<Receiver>();
                    liveReceivers.Add(keyId, receivers);
                }

                if (commandId.Type.IsSubclassOf(typeof(TargetedCommand)))
                {
                    if (receivers.Count > 0 && !receivers.Contains(receiver))
                    {
                        //This should not happen
                        System.Diagnostics.Debug.Assert(false, $"{keyId}: Another noticeReceiver is already registered on this notice !!!!");
                    }
                }

                receivers.Add(receiver);

                this.Get<DebugMenuDocument>().Section<LiveReceiverSection>().AddCommandType(commandId.Type);
            }
        }

        private void Unregister(Receiver receiver)
        {
            foreach (var commandId in receiver.ActionsToUnregister)
            {
                if (liveReceivers.TryGetValue(commandId.Key, out var receivers))
                {
                    receivers.Remove(receiver);
                }
            }
        }

        public void SetupDebugDocument(DebugMenuDocument document, out string title)
        {
            title = "Command daemon";

            var servants = new DaemonOverseerSection<CommandDaemon, CommandServant>();
            var receivers = new LiveReceiverSection("Live receivers");

            document.AddSections(servants);
            document.AddSections(receivers);
        }
        #endregion
    }
    #endregion
}

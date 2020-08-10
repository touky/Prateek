namespace Prateek.Runtime.CommandFramework
{
    using System.Collections.Generic;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.Debug;
    using Prateek.Runtime.CommandFramework.EmitterReceiver;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.DaemonFramework;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Interfaces;
    using Prateek.Runtime.GadgetFramework;
    using Prateek.Runtime.TickableFramework.Interfaces;

    #region Nested type: MessageService
    public sealed class CommandDaemon
        : DaemonOverseer<CommandDaemon, CommandServant>
        , IDebugMenuDocumentOwner
        , IEarlyUpdateTickable
    {
        #region Fields
        private readonly object noticeLock = new object();
        private Dictionary<long, HashSet<CommandReceiver>> liveReceivers;

        private DefaultCommandEmitter defaultEmitter;

        private List<Command> commandReceived;
        private List<Command> commandCached;

        private LiveReceiverSection debugLivePage = null;
        #endregion

        #region Properties
        public static ICommandEmitter DefaultEmitter { get { return Instance.defaultEmitter.Emitter; } }
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
        #endregion

        #region Service
        protected override void OnAwake()
        {
            base.OnAwake();

            if (liveReceivers == null)
            {
                liveReceivers = new Dictionary<long, HashSet<CommandReceiver>>();
            }

            commandCached = new List<Command>();

            lock (noticeLock)
            {
                commandReceived = new List<Command>();
            }

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

                this.Get<DebugMenuDocument>().Section<LiveReceiverSection>().AddCommandType(commandId.Type);
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

        public void SetupDebugDocument(DebugMenuDocument document, out string title)
        {
            title = "Command daemon";

            debugLivePage = new LiveReceiverSection("Live receivers");

            document.AddSections(debugLivePage);
        }
        #endregion
    }
    #endregion
}

namespace Prateek.CommandFramework.TransmitterReceiver {
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.CommandFramework.Servants;
    using Commands.Core;
    using Prateek.Core.Code.Extensions;
    using Prateek.Core.Code.Singleton;

    [DebuggerDisplay("Owner: {owner.Name}")]
    internal sealed class CommandReceiver : ICommandReceiver
    {
        #region Fields
        private ICommandReceiverOwner owner;
        private Action onCommandReceived;

        private bool commandReadLock = false;
        private List<Command> receivedCommands = new List<Command>();
        private List<Command> processingCommands = new List<Command>();
        private Dictionary<long, ICommandCallbackProxy> commandCallbacks = new Dictionary<long, ICommandCallbackProxy>();

        private List<CommandId> callbackToRegister = new List<CommandId>();
        private List<CommandId> callbackToUnregister = new List<CommandId>();
        #endregion

        #region Properties
        public List<CommandId> CallbackToRegister
        {
            get { return callbackToRegister; }
        }

        public List<CommandId> CallbackToUnregister
        {
            get { return callbackToUnregister; }
        }
        #endregion

        #region Constructors
        public CommandReceiver(ICommandReceiverOwner owner)
        {
            this.owner = owner;
        }
        #endregion

        #region Class Methods
        public void Receive(Command receivedCommand)
        {
            //Disallow any noticeReceiver from receiving their notices now
            //MessageReceived() callback is only meant for async purpose
            if (receivedCommands.Count == 0)
            {
                commandReadLock = true;
                {
                    onCommandReceived.SafeInvoke();
                }
                commandReadLock = false;
            }

            receivedCommands.Add(receivedCommand);
        }
        #endregion

        #region IMessageCommunicator Members
        public void CleanUp()
        {
            if (SingletonBehaviour<CommandDaemon>.IsApplicationQuitting)
            {
                return;
            }

            ClearCallbacks();
            ApplyCallbacks();
        }

        public ICommandReceiverOwner Owner
        {
            get { return owner; }
        }
        #endregion

        #region Sending
        public void Broadcast(BroadcastCommand command)
        {
            Send(command);
        }

        public void Send(TargetedCommand command)
        {
            Send((Command) command);
        }

        public void Send(DirectCommand command)
        {
            Send((Command) command);
        }

        public void Send(ResponseCommand command)
        {
            UnityEngine.Debug.Assert(command != null, "command != null");
            UnityEngine.Debug.Assert(command.Recipient != null, $"command.Recipient != null, Type: {command.GetType().Name}");

            Send((Command) command);
        }

        private void Send(Command command)
        {
            UnityEngine.Debug.Assert(command != null);

            command.Emitter = this;

            CommandDaemon.ReceiveMessage(command);
        }
        #endregion

        #region Receiving
        public void ProcessAllCommands()
        {
            if (receivedCommands.Count == 0)
            {
                return;
            }

            if (commandReadLock)
            {
                UnityEngine.Debug.Assert(false, "ProcessAllMessages() call is forbidden on MessageReceived()");
                return;
            }

            processingCommands.AddRange(receivedCommands);
            receivedCommands.Clear();

            ICommandCallbackProxy callback = null;
            foreach (Command command in processingCommands)
            {
                long commandId = command.CommandID;
                if (commandCallbacks.TryGetValue(commandId, out callback))
                {
                    callback.Invoke(command);
                }
                else
                {
                    //This can happen, if no noticeReceiver has been registered to this type of event
                    //We log a warning for now (18/10/19) since this is not supposed to be a problem in this system logic
                    //todo DebugTools.LogWarning($"No recipient found for notice {notice.GetType().Name} sent by {notice.Sender.Owner.Name}");
                }
            }

            processingCommands.Clear();
        }
        #endregion

        #region Callback management
        public void SetCommandReceived(Action onCommandReceived)
        {
            this.onCommandReceived = onCommandReceived;
        }

        public void AddCallback<T>(CommandCallback<T> callback) where T : Command
        {
            CommandId register = new CommandId(typeof(T), this);

            InternalAddCallback(register, new CommandCallbackProxy<T>(callback));

            SetCallbackAsPending(register, true);
        }

        public void RemoveCallback<T>() where T : Command
        {
            CommandId id = new CommandId(typeof(T), this);

            SetCallbackAsPending(id, false);
        }

        public void ClearCallbacks()
        {
            foreach (long key in commandCallbacks.Keys)
            {
                SetCallbackAsPending(key, false);
            }
        }

        public void ApplyCallbacks()
        {
            CommandDaemon.RefreshRegistration(this);

            CallbackToRegister.Clear();
            CallbackToUnregister.Clear();
        }

        private void InternalAddCallback(CommandId register, ICommandCallbackProxy callbackProxy)
        {
            long id = register.GetValidId();

            if (!commandCallbacks.ContainsKey(id))
            {
                commandCallbacks.Add(id, callbackProxy);
            }
            else
            {
                commandCallbacks[id] = callbackProxy;
            }
        }

        private void SetCallbackAsPending(CommandId id, bool doRegister)
        {
            List<CommandId> removeList = doRegister ? callbackToUnregister : callbackToRegister;
            List<CommandId> addList    = doRegister ? callbackToRegister : callbackToUnregister;

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
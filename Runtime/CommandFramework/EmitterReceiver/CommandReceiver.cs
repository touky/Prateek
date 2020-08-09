﻿namespace Prateek.Runtime.CommandFramework.EmitterReceiver
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Singleton;

    [DebuggerDisplay("Owner: {owner.Name}")]
    internal sealed class CommandReceiver : ICommandReceiver
    {
        #region Fields
        private ICommandReceiverOwner owner;
        private Action onCommandReceived;

        private bool commandReadLock = false;
        private List<Command> commandReceived = new List<Command>();
        private List<Command> commandCached = new List<Command>();
        private Dictionary<long, ICommandActionProxy> commandActions = new Dictionary<long, ICommandActionProxy>();

        private List<CommandId> actionsToRegister = new List<CommandId>();
        private List<CommandId> actionsToUnregister = new List<CommandId>();
        #endregion

        #region Properties
        public List<CommandId> ActionsToRegister { get { return actionsToRegister; } }

        public List<CommandId> ActionsToUnregister { get { return actionsToUnregister; } }
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
            if (commandReceived.Count == 0)
            {
                commandReadLock = true;
                {
                    onCommandReceived.SafeInvoke();
                }
                commandReadLock = false;
            }

            commandReceived.Add(receivedCommand);
        }
        #endregion

        #region ICommandReceiver Members
        public void Kill()
        {
            if (SingletonBehaviour<CommandDaemon>.IsApplicationQuitting)
            {
                return;
            }

            ClearAllActions();
            ApplyActionChanges();
        }

        public ICommandReceiverOwner Owner { get { return owner; } }

        #region Receiving
        public void ProcessReceivedCommands()
        {
            if (commandReceived.Count == 0)
            {
                return;
            }

            if (commandReadLock)
            {
                UnityEngine.Debug.Assert(false, "ProcessReceivedCommands() call is forbidden during Receive() call");
                return;
            }

            commandCached.AddRange(commandReceived);
            commandReceived.Clear();

            foreach (var command in commandCached)
            {
                var commandId = command.CommandId;
                if (commandActions.TryGetValue(commandId.Key, out var action))
                {
                    action.Invoke(command);
                }
                else
                {
                    //This can happen, if no noticeReceiver has been registered to this type of event
                    //We log a warning for now (18/10/19) since this is not supposed to be a problem in this system logic
                    //todo DebugTools.LogWarning($"No recipient found for notice {notice.GetType().Name} sent by {notice.Sender.Owner.Name}");
                }
            }

            commandCached.Clear();
        }
        #endregion
        #endregion

        #region Sending
        public void Send(BroadcastCommand command)
        {
            Send((Command) command);
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

            CommandDaemon.CommandReceived(command);
        }
        #endregion

        #region Callback management
        public void SetActionForReception(Action onCommandReceived)
        {
            this.onCommandReceived = onCommandReceived;
        }

        public void SetActionFor<T>(CommandAction<T> action) where T : Command
        {
            var commandId = new CommandId(typeof(T), this);

            AddActionForCommand(commandId, new CommandActionProxy<T>(action));

            AddPendingCommandId(commandId, true);
        }

        public void ClearActionFor<T>() where T : Command
        {
            var commandId = new CommandId(typeof(T), this);

            AddPendingCommandId(commandId, false);
        }

        public void ClearAllActions()
        {
            foreach (var key in commandActions.Keys)
            {
                AddPendingCommandId(key, false);
            }
        }

        public void ApplyActionChanges()
        {
            CommandDaemon.FlushPendingActions(this);

            ActionsToRegister.Clear();
            ActionsToUnregister.Clear();
        }

        private void AddActionForCommand(CommandId commandId, ICommandActionProxy actionProxy)
        {
            var id = commandId.Key;

            if (!commandActions.ContainsKey(id))
            {
                commandActions.Add(id, actionProxy);
            }
            else
            {
                commandActions[id] = actionProxy;
            }
        }

        private void AddPendingCommandId(CommandId id, bool needToRegister)
        {
            var removeList = needToRegister ? actionsToUnregister : actionsToRegister;
            var addList    = needToRegister ? actionsToRegister : actionsToUnregister;

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

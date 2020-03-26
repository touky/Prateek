namespace Assets.Prateek.ToConvert.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using Assets.Prateek.ToConvert.DebugMenu.Content;
    using Assets.Prateek.ToConvert.DebugMenu.Pages;
    using Assets.Prateek.ToConvert.Messaging.Communicator;
    using Assets.Prateek.ToConvert.Messaging.Messages;
    using Assets.Prateek.ToConvert.Reflection;
    using Assets.Prateek.ToConvert.Service;
    using UnityEngine;

    #region Nested type: MessageService
    public sealed class MessageService : ServiceSingletonBehaviour<MessageService, MessageServiceProvider>, IDebugMenuNotebookOwner
    {
        #region Fields
        private Dictionary<long, HashSet<MessageCommunicator>> liveCommunicators;
        private readonly object messageLock = new object();
        private List<Message> receivedMessages;
        private List<Message> processedMessages;
        private MessageServiceDefaultCommunicator defaultCommunicator;

        private LiveCommunicatorsMenuPage debugLivePage = null;
        #endregion

        #region Properties
        public static ILightMessageCommunicator DefaultCommunicator
        {
            get { return Instance.defaultCommunicator.Communicator; }
        }
        #endregion

        #region Unity Methods
        private void Update()
        {
            ProcessMessages();
        }
        #endregion

        #region Service
        protected override void OnAwake()
        {
            if (liveCommunicators == null)
            {
                liveCommunicators = new Dictionary<long, HashSet<MessageCommunicator>>();
            }

            processedMessages = new List<Message>();

            lock (messageLock)
            {
                receivedMessages = new List<Message>();
            }

            SetupDebugContent();

            defaultCommunicator = new MessageServiceDefaultCommunicator();
        }

        private void Register(MessageCommunicator communicator)
        {
            HashSet<MessageCommunicator> communicators = null;
            foreach (var register in communicator.CallbackToRegister)
            {
                var messageId = register.GetValidId();
                if (liveCommunicators.TryGetValue(messageId, out communicators))
                {
                    if (register.Type.IsSubclassOf(typeof(TargetedMessage)))
                    {
                        if (communicators.Count > 0 && !communicators.Contains(communicator))
                        {
                            //This should not happen
                            System.Diagnostics.Debug.Assert(false, $"{messageId}: Another communicator is already registered on this message !!!!");

                            continue;
                        }
                    }

                    communicators.Add(communicator);
                }
                else
                {
                    communicators = new HashSet<MessageCommunicator>();
                    communicators.Add(communicator);
                    liveCommunicators.Add(register.GetValidId(), communicators);

                    AddType(register.Type);
                }
            }
        }
        #endregion

        #region Class Methods
        public static IMessageCommunicator CreateNewCommunicator(IMessageCommunicatorOwner owner)
        {
            return new MessageCommunicator(owner);
        }

        public void ReceiveMessage(Message receivedMessage)
        {
            var provider = GetFirstValidProvider();
            if (provider == null)
            {
                return;
            }

            provider.ReceiveMessage(this, receivedMessage);
        }

        public void AddMessage(Message receivedMessage)
        {
            lock (messageLock)
            {
                receivedMessages.Add(receivedMessage);
            }
        }

        private void ProcessMessages()
        {
            //Emptying the received messages to ensure that no message received in-between processing interferes with the processing
            lock (messageLock)
            {
                if (receivedMessages.Count == 0)
                {
                    return;
                }

                processedMessages.AddRange(receivedMessages);
                receivedMessages.Clear();
            }

            StringBuilder builder = null;

            HashSet<MessageCommunicator> communicators = null;
            foreach (var message in processedMessages)
            {
                builder.AddReceivedMessage(this, message);

                //Standard message management, send to concerned communicators
                var messageId = message.MessageID;
                if (liveCommunicators.TryGetValue(messageId, out communicators))
                {
                    foreach (var communicator in communicators)
                    {
                        builder.AddCommunicator(this, communicator);

                        communicator.Receive(message);

                        //if (connectionDrawer == null)
                        //{
                        //    connectionDrawer = new GameObject().AddComponent<MessageServiceDrawer>();
                        //}
                        //else
                        //{
                        //    connectionDrawer.Tag(communicator, message);
                        //}
                    }
                }
            }

            //DebugTools.Log(builder, //DebugTools.LogLevel.Verbose);

            processedMessages.Clear();
        }

        private void RefreshRegistration(MessageCommunicator communicator)
        {
            Unregister(communicator);
            Register(communicator);
        }

        private void Unregister(MessageCommunicator communicator)
        {
            HashSet<MessageCommunicator> communicators = null;
            foreach (var register in communicator.CallbackToUnregister)
            {
                if (liveCommunicators.TryGetValue(register.GetValidId(), out communicators))
                {
                    communicators.Remove(communicator);
                }
            }
        }
        #endregion

        #region Nested type: CommunicatorId
        private struct CommunicatorId
        {
            private long Id;
            private Type type;
            private MessageCommunicator communicator;

            public Type Type
            {
                get { return type; }
            }

            public CommunicatorId(Type type, MessageCommunicator communicator)
            {
                this.type = type;
                this.communicator = communicator;
                Id = 0;
            }

            public static implicit operator CommunicatorId(Type type)
            {
                return new CommunicatorId(type, null);
            }

            public static implicit operator CommunicatorId(long Id)
            {
                return new CommunicatorId {Id = Id};
            }

            public long GetValidId()
            {
                if (Id != 0)
                {
                    return Id;
                }

                if (type.IsSubclassOf(typeof(ResponseMessage)))
                {
                    Id = Message.ConvertToId(type, communicator);
                }
                else
                {
                    Id = Message.ConvertToId(type);
                }

                return Id;
            }
        }
        #endregion

        #region Nested type: MessageCommunicator
        [DebuggerDisplay("Owner: {owner.Name}")]
        private sealed class MessageCommunicator : IMessageCommunicator
        {
            #region Fields
            private IMessageCommunicatorOwner owner;
            private bool messageReadLock = false;
            private List<Message> receivedMessages = new List<Message>();
            private List<Message> processedMessages = new List<Message>();
            private Dictionary<long, IMessageCallbackProxy> messageCallbacks = new Dictionary<long, IMessageCallbackProxy>();

            private List<CommunicatorId> callbackToRegister = new List<CommunicatorId>();
            private List<CommunicatorId> callbackToUnregister = new List<CommunicatorId>();
            #endregion

            #region Properties
            public List<CommunicatorId> CallbackToRegister
            {
                get { return callbackToRegister; }
            }

            public List<CommunicatorId> CallbackToUnregister
            {
                get { return callbackToUnregister; }
            }
            #endregion

            #region Constructors
            public MessageCommunicator(IMessageCommunicatorOwner owner)
            {
                this.owner = owner;
            }
            #endregion

            #region Class Methods
            public void Receive(Message receivedMessage)
            {
                //Disallow any communicator from receiving their messages now
                //MessageReceived() callback is only meant for async purpose
                messageReadLock = true;
                {
                    owner.MessageReceived();
                }
                messageReadLock = false;

                receivedMessages.Add(receivedMessage);
            }
            #endregion

            #region IMessageCommunicator Members
            public void CleanUp()
            {
                if (ApplicationIsQuitting)
                {
                    return;
                }

                ClearCallbacks();
                ApplyCallbacks();
            }

            public IMessageCommunicatorOwner Owner
            {
                get { return owner; }
            }
            #endregion

            #region Sending
            public void Broadcast(BroadcastMessage message)
            {
                Send(message);
            }

            public void Send(TargetedMessage message)
            {
                Send((Message) message);
            }

            public void Send(DirectMessage message)
            {
                Send((Message) message);
            }

            public void Send(ResponseMessage message)
            {
                UnityEngine.Debug.Assert(message != null, "message != null");
                UnityEngine.Debug.Assert(message.Recipient != null, $"message.Recipient != null, Type: {message.GetType().Name}");

                Send((Message) message);
            }

            private void Send(Message message)
            {
                UnityEngine.Debug.Assert(message != null);

                message.Sender = this;

                Instance.ReceiveMessage(message);
            }
            #endregion

            #region Receiving
            public bool HasMessage()
            {
                return receivedMessages.Count > 0;
            }

            public void ProcessAllMessages()
            {
                if (messageReadLock)
                {
                    UnityEngine.Debug.Assert(false, "ProcessAllMessages() call is forbidden on MessageReceived()");
                    return;
                }

                processedMessages.AddRange(receivedMessages);
                receivedMessages.Clear();

                IMessageCallbackProxy callback = null;
                foreach (var message in processedMessages)
                {
                    var messageId = message.MessageID;
                    if (messageCallbacks.TryGetValue(messageId, out callback))
                    {
                        callback.Invoke(message);
                    }
                    else
                    {
                        //This can happen, if no communicator has been registered to this type of event
                        //We log a warning for now (18/10/19) since this is not supposed to be a problem in this system logic
                        //DebugTools.LogWarning($"No recipient found for message {message.GetType().Name} sent by {message.Sender.Owner.Name}");
                    }
                }

                processedMessages.Clear();
            }
            #endregion

            #region Callback management
            public void AddCallback<T>(MessageCallback<T> messageCallback) where T : Message
            {
                var register = new CommunicatorId(typeof(T), this);

                InternalAddCallback(register, new MessageCallbackProxy<T>(messageCallback));

                SetCallbackAsPending(register, true);
            }

            public void RemoveCallback<T>() where T : Message
            {
                var id = new CommunicatorId(typeof(T), this);

                SetCallbackAsPending(id, false);
            }

            public void ClearCallbacks()
            {
                foreach (var key in messageCallbacks.Keys)
                {
                    SetCallbackAsPending(key, false);
                }
            }

            public void ApplyCallbacks()
            {
                Instance.RefreshRegistration(this);

                CallbackToRegister.Clear();
                CallbackToUnregister.Clear();
            }

            private void InternalAddCallback(CommunicatorId register, IMessageCallbackProxy callbackProxy)
            {
                var id = register.GetValidId();

                if (!messageCallbacks.ContainsKey(id))
                {
                    messageCallbacks.Add(id, callbackProxy);
                }
                else
                {
                    messageCallbacks[id] = callbackProxy;
                }
            }

            private void SetCallbackAsPending(CommunicatorId id, bool doRegister)
            {
                var removeList = doRegister ? callbackToUnregister : callbackToRegister;
                var addList    = doRegister ? callbackToRegister : callbackToUnregister;

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
        #endregion

        #region Nested type: MessageServiceDefaultCommunicator
        private class MessageServiceDefaultCommunicator : IMessageCommunicatorOwner
        {
            #region Fields
            private IMessageCommunicator communicator;
            #endregion

            #region Constructors
            public MessageServiceDefaultCommunicator()
            {
                communicator = CreateNewCommunicator(this);
            }
            #endregion

            #region IMessageCommunicatorOwner Members
            public IMessageCommunicator Communicator
            {
                get { return communicator; }
            }

            public string Name
            {
                get { return "MessageServiceCommunicator"; }
            }

            public Transform Transform
            {
                get { return null; }
            }

            public void MessageReceived()
            {
                throw new NotImplementedException($"Cannot receiver messages through the {typeof(MessageServiceDefaultCommunicator).Name}");
            }
            #endregion
        }
        #endregion

        #region Debug
        [Conditional("NVIZZIO_DEV")]
        private void SetupDebugContent()
        {
            var debugNotebook = new DebugMenuNotebook("MSGS", "Message Service");
            debugLivePage = new LiveCommunicatorsMenuPage(this, "Live communicators");

            debugNotebook.AddPagesWithParent(new EmptyMenuPage("MAIN"), debugLivePage);
            debugNotebook.Register();
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

        #region Nested type: LiveCommunicatorsMenuPage
        //Can't do without this class, as MessageCommunicator is purely private
        public abstract class InternalLiveCommunicatorsMenuPage : DebugMenuPage<MessageService>
        {
            #region Fields
            private ReflectedField<Dictionary<long, HashSet<MessageCommunicator>>> liveCommunicators = "liveCommunicators";
            #endregion

            #region Properties
            protected int KeyCount
            {
                get { return liveCommunicators.Value.Keys.Count; }
            }

            protected long this[int index]
            {
                get
                {
                    var                                                          k    = 0;
                    Dictionary<long, HashSet<MessageCommunicator>>.KeyCollection keys = liveCommunicators.Value.Keys;
                    foreach (var key in keys)
                    {
                        if (k++ == index)
                        {
                            return key;
                        }
                    }

                    return 0;
                }
            }
            #endregion

            #region Constructors
            public InternalLiveCommunicatorsMenuPage(MessageService owner, string title) : base(owner, title)
            {
                Dictionary<long, HashSet<MessageCommunicator>>.KeyCollection keys = liveCommunicators.Value.Keys;
            }
            #endregion

            #region Class Methods
            protected int GetCommunicatorCount(long key)
            {
                HashSet<MessageCommunicator> communicators = null;
                if (liveCommunicators.Value.TryGetValue(key, out communicators))
                {
                    return communicators.Count;
                }

                return 0;
            }

            protected IMessageCommunicator GetCommunicator(long key, int index)
            {
                HashSet<MessageCommunicator> communicators = null;
                if (liveCommunicators.Value.TryGetValue(key, out communicators))
                {
                    var k = 0;
                    foreach (var communicator in communicators)
                    {
                        if (k++ == index)
                        {
                            return communicator;
                        }
                    }
                }

                return null;
            }
            #endregion
        }
        #endregion
        #endregion
    }
    #endregion
}

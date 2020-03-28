namespace Mayfair.Core.Code.Messaging
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.Messaging.Communicator;
    using Mayfair.Core.Code.Messaging.Messages;
    using Mayfair.Core.Code.Utils;

    internal class LiveCommunicatorsMenuPage : MessageDaemonCore.InternalLiveCommunicatorsMenuPage
    {
        #region Fields
        private Dictionary<long, Type> messageTypes = new Dictionary<long, Type>();
        #endregion

        #region Constructors
        public LiveCommunicatorsMenuPage(MessageDaemonCore owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        public void AddType(Type type)
        {
            long validId = Message.ConvertToId(type);
            if (messageTypes.ContainsKey(validId))
            {
                return;
            }

            messageTypes.Add(validId, type);
        }

        protected override void Draw(MessageDaemonCore owner, DebugMenuContext context)
        {
            for (int k = 0; k < KeyCount; k++)
            {
                long key = this[k];
                string messageName = messageTypes.ContainsKey(key & Message.ID_TYPE_MASK) ? messageTypes[key & Message.ID_TYPE_MASK].Name : Consts.UNDEFINED;

                CategoryField categoryField = GetField<CategoryField>();
                categoryField.Draw(context, string.Format("{1}: {0:X}", key, messageName));
                if (categoryField.ShowContent)
                {
                    using (new ContextIndentScope(context, 1))
                    {
                        int count = GetCommunicatorCount(key);
                        if (count > 0)
                        {
                            for (int c = 0; c < count; c++)
                            {
                                IMessageCommunicator messageCommunicator = GetCommunicator(key, c);
                                LabelField labelField = GetField<LabelField>();
                                labelField.Draw(context, $"Owner: {messageCommunicator.Owner.Name}");
                            }
                        }
                        else
                        {
                            LabelField labelField = GetField<LabelField>();
                            labelField.Draw(context, "-EMPTY-");
                        }
                    }
                }
            }
        }
        #endregion
    }
}

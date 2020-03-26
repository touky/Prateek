namespace Assets.Prateek.ToConvert.Messaging
{
    using System;
    using System.Collections.Generic;
    using Assets.Prateek.ToConvert.DebugMenu;
    using Assets.Prateek.ToConvert.DebugMenu.Fields;
    using Assets.Prateek.ToConvert.Messaging.Messages;

    internal class LiveCommunicatorsMenuPage : MessageService.InternalLiveCommunicatorsMenuPage
    {
        #region Fields
        private Dictionary<long, Type> messageTypes = new Dictionary<long, Type>();
        #endregion

        #region Constructors
        public LiveCommunicatorsMenuPage(MessageService owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        public void AddType(Type type)
        {
            var validId = Message.ConvertToId(type);
            if (messageTypes.ContainsKey(validId))
            {
                return;
            }

            messageTypes.Add(validId, type);
        }

        protected override void Draw(MessageService owner, DebugMenuContext context)
        {
            for (var k = 0; k < KeyCount; k++)
            {
                var key         = this[k];
                var messageName = messageTypes.ContainsKey(key & Message.ID_TYPE_MASK) ? messageTypes[key & Message.ID_TYPE_MASK].Name : Consts.UNDEFINED;

                var categoryField = GetField<CategoryField>();
                categoryField.Draw(context, string.Format("{1}: {0:X}", key, messageName));
                if (categoryField.ShowContent)
                {
                    using (new ContextIndentScope(context, 1))
                    {
                        var count = GetCommunicatorCount(key);
                        if (count > 0)
                        {
                            for (var c = 0; c < count; c++)
                            {
                                var messageCommunicator = GetCommunicator(key, c);
                                var labelField          = GetField<LabelField>();
                                labelField.Draw(context, $"Owner: {messageCommunicator.Owner.Name}");
                            }
                        }
                        else
                        {
                            var labelField = GetField<LabelField>();
                            labelField.Draw(context, "-EMPTY-");
                        }
                    }
                }
            }
        }
        #endregion
    }
}

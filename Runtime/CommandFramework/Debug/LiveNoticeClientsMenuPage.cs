namespace Prateek.Runtime.CommandFramework.Debug
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.DebugMenu;
    using Mayfair.Core.Code.DebugMenu.Fields;
    using Mayfair.Core.Code.DebugMenu.Pages;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;

    internal class LiveNoticeReceiversMenuPage : DebugMenuPage<CommandDaemon>
    {
        #region Fields
        private ReflectedField<Dictionary<long, HashSet<CommandReceiver>>> liveClients = "liveClients";
        private Dictionary<long, Type> noticeTypes = new Dictionary<long, Type>();
        #endregion

        #region Properties
        protected int KeyCount { get { return liveClients.Value.Keys.Count; } }

        protected long this[int index]
        {
            get
            {
                var k    = 0;
                var keys = liveClients.Value.Keys;
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
        public LiveNoticeReceiversMenuPage(CommandDaemon owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected int GetNoticeReceiverCount(long key)
        {
            if (liveClients.Value.TryGetValue(key, out var receivers))
            {
                return receivers.Count;
            }

            return 0;
        }

        protected ICommandReceiver GetNoticeReceiver(long key, int index)
        {
            HashSet<CommandReceiver> receivers = null;
            if (liveClients.Value.TryGetValue(key, out receivers))
            {
                var k = 0;
                foreach (var noticeReceiver in receivers)
                {
                    if (k++ == index)
                    {
                        return noticeReceiver;
                    }
                }
            }

            return null;
        }

        public void AddType(Type type)
        {
            var validId = (CommandId) type;
            if (noticeTypes.ContainsKey(validId.Key))
            {
                return;
            }

            noticeTypes.Add(validId.Key, type);
        }

        protected override void Draw(CommandDaemon owner, DebugMenuContext context)
        {
            for (var k = 0; k < KeyCount; k++)
            {
                var key        = this[k];
                var noticeName = noticeTypes.ContainsKey(key & CommandId.MASK_TYPE) ? noticeTypes[key & CommandId.MASK_TYPE].Name : "#UNDEFINED#"; //todo Consts.UNDEFINED;

                var categoryField = GetField<CategoryField>();
                categoryField.Draw(context, string.Format("{1}: {0:X}", key, noticeName));
                if (categoryField.ShowContent)
                {
                    using (new ContextIndentScope(context))
                    {
                        var count = GetNoticeReceiverCount(key);
                        if (count > 0)
                        {
                            for (var c = 0; c < count; c++)
                            {
                                var noticeReceiver = GetNoticeReceiver(key, c);
                                var labelField     = GetField<LabelField>();
                                labelField.Draw(context, $"Owner: {noticeReceiver.Owner.Name}");
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

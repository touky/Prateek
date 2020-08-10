namespace Prateek.Runtime.CommandFramework.Debug
{
    using System;
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.CommandFramework.Commands.Core;
    using Prateek.Runtime.CommandFramework.EmitterReceiver;
    using Prateek.Runtime.CommandFramework.EmitterReceiver.Interfaces;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.Reflection;

    internal class LiveReceiverSection
        : DebugMenuSection<CommandDaemon>
    {
        #region Fields
        private DebugField<Dictionary<long, HashSet<CommandReceiver>>> receivers = "liveReceivers";
        private Dictionary<long, Type> commandTypes = new Dictionary<long, Type>();
        #endregion

        #region Constructors
        public LiveReceiverSection(string title) : base(title) { }
        #endregion

        #region Class Methods
        public void AddType(Type type)
        {
            var validId = (CommandId) type;
            if (commandTypes.ContainsKey(validId.Key))
            {
                return;
            }

            commandTypes.Add(validId.Key, type);
        }

        protected override void OnDraw(DebugMenuContext context)
        {
            if (!receivers.AssertDrawable())
            {
                return;
            }

            for (var k = 0; k < receivers.Value.Keys.Count; k++)
            {
                var key     = Key(k);
                var cmdName = commandTypes.ContainsKey(key & CommandId.MASK_TYPE) ? commandTypes[key & CommandId.MASK_TYPE].Name : Const.UNDEFINED;

                if (ImGui.CollapsingHeader(string.Format("{1}: {0:X}", key, cmdName)))
                {
                    ImGui.Indent();
                    {
                        var count = GetReceiverCount(key);
                        if (count > 0)
                        {
                            for (var c = 0; c < count; c++)
                            {
                                var noticeReceiver = GetReceiver(key, c);
                                ImGui.Text($"Owner: {noticeReceiver.Owner.Name}");
                            }
                        }
                        else
                        {
                            ImGui.Text(Const.EMPTY);
                        }
                    }
                    ImGui.Unindent();
                }
            }
        }

        protected int GetReceiverCount(long key)
        {
            if (this.receivers.Value.TryGetValue(key, out var receivers))
            {
                return receivers.Count;
            }

            return 0;
        }

        protected ICommandReceiver GetReceiver(long key, int index)
        {
            if (this.receivers.Value.TryGetValue(key, out var receivers))
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

        protected long Key(int index)
        {
            var k = 0;
            foreach (var key in receivers.Value.Keys)
            {
                if (k++ == index)
                {
                    return key;
                }
            }

            return 0;
        }
        #endregion
    }
}

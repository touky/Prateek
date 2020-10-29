namespace Prateek.Runtime.CommandFramework.Debug
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.CommandFramework.Servants;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.Reflection;

    internal class LiveServantsSection
        : DebugMenuSection<CommandDaemon>
    {
        #region Fields
        private DebugField<List<CommandServant>> servants = "servants";
        #endregion

        #region Constructors
        public LiveServantsSection(string title)
            : base(title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            DrawServants(context);
        }

        private void DrawServants(DebugMenuContext context)
        {
            if (!servants.AssertDrawable())
            {
                return;
            }

            foreach (var commandServant in servants.Value)
            {
                var text = $"[{(commandServant.IsAlive ? "ON" : "OFF")}] ({commandServant.GetType().Name}) {commandServant.Name}";
                if (commandServant.IsAlive)
                {
                    ImGui.Text(text);
                }
                else
                {
                    ImGui.TextDisabled(text);
                }
            }
        }
        #endregion
    }
}

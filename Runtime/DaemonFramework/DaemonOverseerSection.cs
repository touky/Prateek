namespace Prateek.Runtime.DaemonFramework
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.DaemonFramework.Interfaces;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Gadgets;
    using Prateek.Runtime.DebugFramework.DebugMenu.Sections;
    using Prateek.Runtime.DebugFramework.Reflection;

    public class DaemonOverseerSection<TDaemonOverseer, TServant>
        : DebugMenuSection<TDaemonOverseer>
        where TDaemonOverseer : DaemonOverseer<TDaemonOverseer, TServant>, DebugMenu.IDebugMenuOwner
        where TServant : class, IServant
    {
        #region Static and Constants
        private const string TITLE = "Registered ";
        #endregion

        #region Fields
        private DebugField<List<TServant>> servants = "servants";
        #endregion

        #region Constructors
        public DaemonOverseerSection()
            : base($"{TITLE}{typeof(TServant).Name}") { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
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

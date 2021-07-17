namespace Prateek.Runtime.DebugFramework.DebugMenu.Sections
{
    using ImGuiNET;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;

    public class DefaultMenuSection : DebugMenuSection
    {
        #region Constructors
        public DefaultMenuSection(string title) : base(title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            ImGui.Text("Nothing to see here");
        }
        #endregion
    }
}

namespace Prateek.Runtime.AppContentFramework.Local.Debug
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.AppContentFramework.Daemons;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.Reflection;

    public class LocalRegistrySection
        : RegistryServantSection<LocalRegistryServant>
    {
        #region Fields
        private DebugField<Dictionary<string, ContentPath>> pathToContentPaths = "pathToContentPaths";
        #endregion

        #region Constructors
        public LocalRegistrySection(LocalRegistryServant owner, string title) : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            base.OnDraw(context);

            ImGui.Separator();
            if (pathToContentPaths.AssertDrawable())
            {
                foreach (var pair in pathToContentPaths.Value)
                {
                    ImGui.Text($"'{pair.Key}': Format '{pair.Value.ContentFormat.GetType().Name}'");
                }
            }
        }
        #endregion
    }
}

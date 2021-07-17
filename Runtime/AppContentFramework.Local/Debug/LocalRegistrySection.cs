namespace Prateek.Runtime.AppContentFramework.Local.Debug
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.AppContentFramework.Daemons.Debug;
    using Prateek.Runtime.AppContentFramework.Local.ContentFormats;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.Reflection;

    public class LocalRegistrySection
        : RegistryServantSection<LocalRegistryServant>
    {
        #region Fields
        private DebugField<Dictionary<string, ContentPath>> pathToContentPaths = "pathToContentPaths";
        private DebugField<List<ContentFormat>> contentFormats = "contentFormats";
        #endregion

        #region Constructors
        public LocalRegistrySection(LocalRegistryServant owner, string title)
            : base(owner, title) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            base.OnDraw(context);

            ImGui.Separator();
            if (contentFormats.DrawHeader("Found Content formats"))
            {
                using (new ScopeIndent())
                {
                    foreach (var contentFormat in contentFormats.Value)
                    {
                        ImGui.Text(contentFormat.GetType().Name);
                    }
                }
            }

            if (pathToContentPaths.DrawHeader("Local content"))
            {
                using (new ScopeIndent())
                {
                    foreach (var pair in pathToContentPaths.Value)
                    {
                        ImGui.Text($"'{pair.Key}': Format '{pair.Value.ContentFormat.GetType().Name}'");
                    }
                }
            }
        }
        #endregion
    }
}

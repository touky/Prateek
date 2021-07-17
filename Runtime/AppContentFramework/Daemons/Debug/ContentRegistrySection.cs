namespace Prateek.Runtime.AppContentFramework.Daemons.Debug
{
    using System.Collections.Generic;
    using ImGuiNET;
    using Prateek.Runtime.AppContentFramework.ContentLoaders;
    using Prateek.Runtime.AppContentFramework.Messages;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.DebugFramework.DebugMenu;
    using Prateek.Runtime.DebugFramework.DebugMenu.Documents;
    using Prateek.Runtime.DebugFramework.DebugMenu.Drawers;
    using Prateek.Runtime.DebugFramework.DebugMenu.Sections;
    using Prateek.Runtime.DebugFramework.Reflection;

    internal class ContentRegistrySection
        : DebugMenuSection<ContentRegistryDaemon>
    {
        #region Static and Constants
        private const string TITLE = "Content registry informations";
        #endregion

        #region Fields
        private DebugField<HierarchicalTree<ContentLoader>> hierarchicalTree = "hierarchicalTree";
        private DebugField<HashSet<ContentAccessRequest>> contentAccessRequests = "contentAccessRequests";
        #endregion

        #region Properties
        public override Setting Settings { get { return Setting.AddSeparatorAfter; } }
        #endregion

        #region Constructors
        public ContentRegistrySection()
            : base(TITLE) { }
        #endregion

        #region Class Methods
        protected override void OnDraw(DebugMenuContext context)
        {
            if (contentAccessRequests.AssertDrawable())
            {
                if (ImGui.CollapsingHeader("Content access requests"))
                {
                    using (new ScopeIndent())
                    {
                        foreach (var accessRequest in contentAccessRequests.Value)
                        {
                            ImGui.Text($"{accessRequest.CommandId.KeyDebugDisplay}: {accessRequest.Emitter.Owner.Name}");
                        }
                    }
                }
            }

            if (hierarchicalTree.AssertDrawable())
            {
                hierarchicalTree.Value.Draw();
            }

            ImGui.Separator();
        }
        #endregion
    }
}

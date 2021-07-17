namespace Prateek.Runtime.DebugFramework.DebugMenu.Drawers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using ImGuiNET;
    using Prateek.Runtime.Core.HierarchicalTree;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;
    using Prateek.Runtime.DebugFramework.Reflection;

    public static class DebugMenuDrawers
    {
        public static void Draw<TLeaf>(this HierarchicalTree<TLeaf> tree, bool drawSettings = true)
            where TLeaf : IHierarchicalTreeLeaf
        {
            if (!drawSettings || ImGui.CollapsingHeader($"Hierarchical Tree"))
            {
                var branchName = (DebugField<string>) "branchName";
                var branches   = (DebugField<Dictionary<string, HierarchicalTree<TLeaf>>>) "branches";
                var leaves     = (DebugField<HashSet<TreeLeaf<IHierarchicalTreeLeaf>>>) "leaves";

                DebugField.SetOwner(tree, branchName, branches, leaves);

                if (drawSettings)
                {
                    var settings = (DebugField<HierarchicalTreeSettingsData>) (tree, "settings");
                    if (!settings.AssertDrawable(branchName, branches, leaves))
                    {
                        return;
                    }

                    var folderRegex = (DebugField<Regex>) "folderRegex" + settings.Value;
                    if (drawSettings && folderRegex.AssertDrawable())
                    {
                        ImGui.Text($"Settings: {folderRegex.Value.ToString()}");
                        ImGui.Separator();
                    }
                }

                using (var branchNode = ImGuiUn.ScopeTreeNode(branchName.Value))
                {
                    if (!branchNode.IsOpen)
                    {
                        return;
                    }

                    if (branches.Value != null)
                    {
                        foreach (var branch in branches.Value.Values)
                        {
                            branch.Draw(false);
                        }
                    }

                    if (leaves.Value != null)
                    {
                        using (var leafNode = branches.Value != null ? ImGuiUn.ScopeTreeNode(leaves.Name) : null)
                        {
                            if (leafNode != null && !leafNode.IsOpen)
                            {
                                return;
                            }

                            foreach (var leaf in leaves.Value)
                            {
                                ImGui.Text($"- {leaf.Name}{leaf.Extension}");
                            }
                        }
                    }
                }
            }
        }
    }
}

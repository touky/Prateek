namespace Prateek.Runtime.Core.HierarchicalTree
{
    using System.Collections.Generic;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;
    using UnityEngine.Assertions;

    public class HierarchicalTree<TLeaf>
        where TLeaf : IHierarchicalTreeLeaf
    {
        #region Fields
        private HierarchicalTreeSettingsData settings = null;
        private string branchName;
        private HierarchicalTree<TLeaf> parent;
        private Dictionary<string, HierarchicalTree<TLeaf>> branches;
        private HashSet<TreeLeaf<IHierarchicalTreeLeaf>> leaves;
        #endregion

        #region Properties
        private bool HasBranches
        {
            get { return branches != null && branches.Count > 0; }
        }

        private Dictionary<string, HierarchicalTree<TLeaf>> Branches
        {
            get
            {
                if (branches == null)
                {
                    branches = new Dictionary<string, HierarchicalTree<TLeaf>>();
                }

                return branches;
            }
        }

        private bool HasResources
        {
            get { return leaves != null && leaves.Count > 0; }
        }

        private HashSet<TreeLeaf<IHierarchicalTreeLeaf>> Leaves
        {
            get
            {
                if (leaves == null)
                {
                    leaves = new HashSet<TreeLeaf<IHierarchicalTreeLeaf>>();
                }

                return leaves;
            }
        }
        #endregion

        #region Constructors
        public HierarchicalTree()
        {
            branchName = "ROOT";
        }

        private HierarchicalTree(HierarchicalTreeSettingsData settings)
            : this()
        {
            Setup(settings);
        }
        #endregion

        #region Class Methods
        public void Setup(HierarchicalTreeSettingsData settings = null)
        {
            this.settings = settings == null ? HierarchicalTreeSettings.Default.Data : settings;
        }

        private void GetSettings(ref HierarchicalTreeSettingsData customSettings)
        {
            Assert.IsNotNull(settings, "settings is null, don't forget to call Setup() to properly setup the tree");

            customSettings = customSettings == null ? settings : customSettings;
        }

        public void Store(TLeaf leadData, HierarchicalTreeSettingsData customSettings = null)
        {
            GetSettings(ref customSettings);

            var parentBranch = this;
            var folderMatches      = customSettings.folderRegex.Matches(leadData.Path);

            for (var m = 0; m < folderMatches.Count; m++)
            {
                var folderMatch = folderMatches[m];
                var value = folderMatch.LastGroup().Value;
                if (m == folderMatches.Count - 1)
                {
                    var name = value;
                    var extension = string.Empty;
                    var nameMatch = customSettings.extensionRegex.Match(value);
                    if (nameMatch.Success)
                    {
                        name = nameMatch.Groups[1].Value;
                        extension = nameMatch.Groups[2].Value;
                    }

                    var leaf = new TreeLeaf<IHierarchicalTreeLeaf>(name, extension, leadData);
                    if (!parentBranch.Leaves.Contains(leaf))
                    {
                        //Remove the old one because the leaf hashCode is Path dependant
                        parentBranch.Leaves.Remove(leaf);
                    }

                    parentBranch.Leaves.Add(leaf);
                    break;
                }

                if (parentBranch.Branches.TryGetValue(value, out var childBranch))
                {
                    parentBranch = childBranch;
                }
                else
                {
                    childBranch = new HierarchicalTree<TLeaf>(customSettings)
                    {
                        parent = parentBranch,
                        branchName = value
                    };

                    parentBranch.Branches.Add(value, childBranch);
                    parentBranch = childBranch;
                }
            }
        }

        public void Remove(IHierarchicalTreeLeaf leadData, HierarchicalTreeSettingsData customSettings = null)
        {
            GetSettings(ref customSettings);

            var activeBranch = SearchForBranch(leadData.Path, true, customSettings);
            if (activeBranch == null)
            {
                return;
            }

            var leaf = new TreeLeaf<IHierarchicalTreeLeaf>(string.Empty, string.Empty, leadData);
            activeBranch.Leaves.Remove(leaf);
        }

        public void Remove(IHierarchicalTreeSearch search)
        {
            Assert.IsTrue(search.SearchPaths.Length > 0, $"Search of type {search.GetType().Name} has no paths to search.");

            foreach (var searchPath in search.SearchPaths)
            {
                var activeBranch = SearchForBranch(searchPath, false, search.Settings);
                if (activeBranch == null)
                {
                    continue;
                }

                activeBranch.Branches.Clear();
                activeBranch.Leaves.Clear();

                if (activeBranch.parent != null)
                {
                    activeBranch.parent.Branches.Remove(activeBranch.branchName);
                }
            }
        }

        public void SearchTree(IHierarchicalTreeSearch search, IHierarchicalTreeSearchResult searchResult)
        {
            Assert.IsTrue(search.SearchPaths.Length > 0, $"Search of type {search.GetType().Name} has no paths to search.");

            foreach (var searchPath in search.SearchPaths)
            {
                var activeBranch = SearchForBranch(searchPath, false, search.Settings);
                if (activeBranch == null)
                {
                    continue;
                }

                RetrieveLeafContent(activeBranch, search, searchResult);
            }
        }

        private HierarchicalTree<TLeaf> SearchForBranch(string searchPath, bool searchPathIsLeaf, HierarchicalTreeSettingsData customSettings)
        {
            GetSettings(ref customSettings);

            var activeBranch = this;
            var matches      = customSettings.folderRegex.Matches(searchPath);
            for (var m = 0; m < matches.Count; m++)
            {
                if (searchPathIsLeaf && m == matches.Count - 1)
                {
                    break;
                }

                var match = matches[m];
                if (!match.Success || activeBranch.branches == null)
                {
                    activeBranch = null;
                    break;
                }

                var folder = match.LastGroup().Value;
                if (!activeBranch.branches.TryGetValue(folder, out var nextBranch))
                {
                    activeBranch = null;
                    break;
                }

                activeBranch = nextBranch;
            }

            return activeBranch;
        }

        private void RetrieveLeafContent(HierarchicalTree<TLeaf> branch, IHierarchicalTreeSearch search, IHierarchicalTreeSearchResult searchResult)
        {
            if (branch.HasResources)
            {
                foreach (var resource in branch.leaves)
                {
                    var extensionIsValid = true;
                    if (search.SearchExtensions != null && search.SearchExtensions.Length > 0)
                    {
                        extensionIsValid = false;
                        foreach (var extension in search.SearchExtensions)
                        {
                            if (resource.Extension == extension)
                            {
                                extensionIsValid = true;
                                break;
                            }
                        }
                    }

                    if (!extensionIsValid)
                    {
                        continue;
                    }

                    if (!search.AcceptLeaf(resource.LeafData))
                    {
                        continue;
                    }

                    searchResult.Add(resource.LeafData);
                }
            }

            if (branch.HasBranches)
            {
                foreach (var childBranch in branch.branches.Values)
                {
                    RetrieveLeafContent(childBranch, search, searchResult);
                }
            }
        }
        #endregion
    }
}

namespace Prateek.A_TODO.Runtime.AppContentFramework.ResourceTree
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class ResourceTree<T> where T : ITreeLeafLocator
    {
        #region Fields
        private Regex tagRegex;
        private string branchTag;
        private Dictionary<string, ResourceTree<T>> servants;
        private HashSet<Leaf> leaves;
        #endregion

        #region Properties
        private bool HasServants
        {
            get { return servants != null && servants.Count > 0; }
        }

        private Dictionary<string, ResourceTree<T>> Servants
        {
            get
            {
                if (servants == null)
                {
                    servants = new Dictionary<string, ResourceTree<T>>();
                }

                return servants;
            }
        }

        private bool HasResources
        {
            get { return leaves != null && leaves.Count > 0; }
        }

        private HashSet<Leaf> Leaves
        {
            get
            {
                if (leaves == null)
                {
                    leaves = new HashSet<Leaf>();
                }

                return leaves;
            }
        }
        #endregion

        #region Constructors
        public ResourceTree(Regex regex)
        {
            tagRegex = regex;
        }
        #endregion

        #region Class Methods
        public void Store(T resource)
        {
            ResourceTree<T> activeBranch = this;
            ResourceTree<T> nextBranch = null;
            MatchCollection collection = tagRegex.Matches(resource.Location);
            foreach (Match match in collection)
            {
                string tag = match.Groups[match.Groups.Count - 1].Value;
                if (activeBranch.Servants.TryGetValue(tag, out nextBranch))
                {
                    activeBranch = nextBranch;
                }
                else
                {
                    nextBranch = new ResourceTree<T>(tagRegex) {branchTag = tag};
                    activeBranch.Servants.Add(tag, nextBranch);
                    activeBranch = nextBranch;
                }
            }

            Leaf leaf = new Leaf(resource);
            if (!activeBranch.Leaves.Contains(leaf))
            {
                activeBranch.Leaves.Add(leaf);
            }
        }

        public void RetrieveResources(ITreeIdentification identification, ITreeIdentificationResult identificationResult)
        {
            ResourceTree<T> nextBranch = null;
            foreach (string[] tags in identification.TreeTags)
            {
                ResourceTree<T> activeBranch = this;
                if (tags == null || tags.Length == 0)
                {
                    continue;
                }

                if (activeBranch.servants == null
                 || !activeBranch.servants.ContainsKey(tags[0])) //todo Consts.FIRST_ITEM
                {
                    continue;
                }

                for (int t = 0; t < tags.Length; t++)
                {
                    string tag = tags[t];
                    if (activeBranch.Servants.TryGetValue(tag, out nextBranch))
                    {
                        activeBranch = nextBranch;
                    }
                    else
                    {
                        break;
                    }
                }

                if (activeBranch != null)
                {
                    RetrieveResources(identification, activeBranch, identificationResult);
                }
                else
                {
                    activeBranch = this;
                }
            }
        }

        private static void RetrieveResources(ITreeIdentification identification, ResourceTree<T> tree, ITreeIdentificationResult identificationResult)
        {
            if (tree.HasResources)
            {
                foreach (Leaf resource in tree.leaves)
                {
                    if (identificationResult.Match(resource.LeafData))
                    {
                        identificationResult.Add(resource.LeafData);
                    }
                }
            }

            if (tree.HasServants)
            {
                foreach (ResourceTree<T> nextBranch in tree.servants.Values)
                {
                    RetrieveResources(identification, nextBranch, identificationResult);
                }
            }
        }
        #endregion

        #region Nested type: Leaf
        private struct Leaf
        {
            private readonly T leafData;

            public T LeafData
            {
                get { return leafData; }
            }

            public Leaf(T leafData)
            {
                this.leafData = leafData;
            }

            public override int GetHashCode()
            {
                return leafData.Location.GetHashCode();
            }
        }
        #endregion
    }
}

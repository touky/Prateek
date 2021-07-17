namespace Prateek.Runtime.Core.HierarchicalTree
{
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    public struct TreeLeaf<TLeaf>
        where TLeaf : IHierarchicalTreeLeaf
    {
        #region Fields
        internal readonly string name;
        internal readonly string extension;
        internal readonly TLeaf leafData;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public string Extension
        {
            get { return extension; }
        }

        public TLeaf LeafData
        {
            get { return leafData; }
        }
        #endregion

        #region Constructors
        public TreeLeaf(string name, string extension, TLeaf leafData)
        {
            this.name = name;
            this.extension = string.IsNullOrEmpty(extension) ? string.Empty : $".{extension.TrimStart('.')}";
            this.leafData = leafData;
        }
        #endregion

        #region Class Methods
        public override int GetHashCode()
        {
            return leafData.Path.GetHashCode();
        }
        #endregion
    }
}

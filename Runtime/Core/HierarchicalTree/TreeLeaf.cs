namespace Prateek.Runtime.Core.HierarchicalTree
{
    using Prateek.Runtime.Core.HierarchicalTree.Interfaces;

    internal struct TreeLeaf<TLeaf> where TLeaf : IHierarchicalTreeLeaf
    {
        #region Fields
        internal readonly string name;
        internal readonly TLeaf leafData;
        #endregion

        #region Properties
        public string Name
        {
            get { return name; }
        }

        public TLeaf LeafData
        {
            get { return leafData; }
        }
        #endregion

        #region Constructors
        public TreeLeaf(string name, TLeaf leafData)
        {
            this.name = name;
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

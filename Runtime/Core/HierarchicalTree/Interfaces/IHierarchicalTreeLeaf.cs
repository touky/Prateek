namespace Prateek.Runtime.Core.HierarchicalTree.Interfaces
{
    public interface IHierarchicalTreeLeaf
    {
        #region Properties
        string Path { get; }
        long Size { get; }
        #endregion
    }
}

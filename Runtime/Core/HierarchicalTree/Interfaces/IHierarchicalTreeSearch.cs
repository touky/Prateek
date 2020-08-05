namespace Prateek.Runtime.Core.HierarchicalTree.Interfaces
{
    public interface IHierarchicalTreeSearch
    {
        #region Properties
        /// <summary>
        ///     Allows to send in custom settings for a search path identification
        /// </summary>
        HierarchicalTreeSettingsData Settings { get; }

        /// <summary>
        ///     Gives the paths that needs to be searched through to validate the search
        /// </summary>
        string[] SearchPaths { get; }
        #endregion

        #region Class Methods
        /// <summary>
        ///     Allows the search to validate the leaf before storing it (in case the type is wrong)
        /// </summary>
        /// <param name="leaf"></param>
        /// <returns></returns>
        bool AcceptLeaf(IHierarchicalTreeLeaf leaf);
        #endregion
    }
}

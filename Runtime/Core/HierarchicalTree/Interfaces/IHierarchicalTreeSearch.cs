namespace Prateek.Runtime.Core.HierarchicalTree.Interfaces
{
    public interface IHierarchicalTreeSearch
    {
        #region Properties
        /// <summary>
        ///     Allows user to send custom settings if the search path use a different setup that the tree one
        /// </summary>
        HierarchicalTreeSettingsData Settings { get; }

        /// <summary>
        ///     The paths that are valid for this search
        /// </summary>
        string[] SearchPaths { get; }

        /// <summary>
        ///     The extensions that are valid for this search, nothing means everything is valid
        /// </summary>
        string[] SearchExtensions { get; }
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

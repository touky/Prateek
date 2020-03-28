namespace Mayfair.Core.Code.Resources.ResourceTree
{
    public interface ITreeIdentificationResult
    {
        #region Class Methods
        /// <summary>
        /// Does the Leaf content match the result container
        /// </summary>
        /// <param name="leafLocator"></param>
        /// <returns></returns>
        bool Match(ITreeLeafLocator leafLocator);
        /// <summary>
        /// Add Leaf content to the result container
        /// </summary>
        /// <param name="leafLocator"></param>
        void Add(ITreeLeafLocator leafLocator);
        #endregion
    }
}

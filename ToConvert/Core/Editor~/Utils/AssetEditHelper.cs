namespace Mayfair.Core.Editor.Utils
{
    using Mayfair.Core.Code.Utils;

    public static class AssetEditHelper
    {
        #region Class Methods
        public static string GetTemporaryFolder(string assetPath)
        {
            return assetPath.Replace($"{ConstsFolders.ASSET}/", $"{ConstsFolders.LIBRARY}/");
        }
        #endregion
    }
}

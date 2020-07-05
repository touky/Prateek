namespace Mayfair.Core.Code.Utils.Helpers.Regexp
{
    using System.Text.RegularExpressions;

    public static class PathRegex
    {
        #region Class Methods
        /// <summary>
        ///     Returns the folder just after the given root
        ///     Null if failed
        /// </summary>
        public static string DetectFolderAfterRoot(string root, string path)
        {
            Regex regex = new Regex(RegexContent.FOLDER_ROOT.Replace(RegexContent.REPLACEMENT, root));
            Match folderAfter = regex.Match(path);
            if (!folderAfter.Success)
            {
                return string.Empty;
            }

            return folderAfter.Groups[Consts.SECOND_ITEM].Value;
        }
        #endregion
    }
}

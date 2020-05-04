namespace Mayfair.Core.Code.Utils.Helpers.Regexp
{
    using System;
    using System.Text.RegularExpressions;

    public static class MeshRegex
    {
        #region Class Methods
        public static int GetLODIndex(string name)
        {
            Match match = RegexContent.LOD_DETECT_REGEX.Match(name);
            if (match.Success)
            {
                Group lastGroup = match.Groups[match.Groups.Count + Consts.PREVIOUS_ITEM];
                return Convert.ToInt32(lastGroup.Value);
            }

            return Consts.INDEX_NONE;
        }
        #endregion
    }
}

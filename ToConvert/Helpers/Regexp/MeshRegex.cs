namespace Assets.Prateek.ToConvert.Helpers.Regexp
{
    using System;

    public static class MeshRegex
    {
        #region Class Methods
        public static int GetLODIndex(string name)
        {
            var match = RegexContent.LOD_DETECT_REGEX.Match(name);
            if (match.Success)
            {
                var lastGroup = match.Groups[match.Groups.Count + Consts.PREVIOUS_ITEM];
                return Convert.ToInt32(lastGroup.Value);
            }

            return Consts.INDEX_NONE;
        }
        #endregion
    }
}

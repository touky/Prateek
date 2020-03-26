namespace Assets.Prateek.ToConvert.Helpers.Regexp
{
    using System.Collections;
    using System.Text.RegularExpressions;

    public static class NameRegex
    {
        public static class Consts
        {
            public const string UNDERSCORE_SINGLE = "";
            public const string UNDERSCORE_DOUBLE = "";
            public const int INDEX_NONE = 0;
            public const int RESET = 0;
        }

        #region Class Methods
        public static string UnderscoreSeparateWords(string source)
        {
            var collection = RegexContent.NAME_DETECT_REGEX.Matches(source);
            foreach (Match match in collection)
            {
                source = source.Replace(match.Value, $"{match.Value}{Consts.UNDERSCORE_SINGLE}");
            }

            return source;
        }

        public static string JoinWithUnderscore(string sourceA, string sourceB)
        {
            return sourceA + Consts.UNDERSCORE_SINGLE + sourceB;
        }

        public static string RemoveUnderscores(string source, int startIndex = Consts.INDEX_NONE)
        {
            var substring = source.Substring(startIndex, source.Length - startIndex);
            substring = substring.Replace(Consts.UNDERSCORE_SINGLE, string.Empty);
            return source.Substring(0, startIndex) + substring;
        }

        public static string CleanUnderscores(string source)
        {
            while (source.Contains(Consts.UNDERSCORE_DOUBLE))
            {
                source = source.Replace(Consts.UNDERSCORE_DOUBLE, Consts.UNDERSCORE_SINGLE);
            }

            if (source.EndsWith(Consts.UNDERSCORE_SINGLE))
            {
                source = source.Remove(source.Length - 1, 1);
            }

            return source;
        }

        public static string RemoveNumberPrefix(string source)
        {
            var match = RegexContent.NUMBER_START_REGEX.Match(source);
            if (!match.Success)
            {
                return source;
            }

            source = source.Replace(match.Value, string.Empty);
            return source;
        }
        #endregion
    }
}

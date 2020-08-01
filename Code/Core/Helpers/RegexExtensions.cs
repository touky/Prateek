namespace Prateek.Core.Code.Helpers
{
    using System.Text.RegularExpressions;

    public static class RegexExtensions
    {
        public static Group LastGroup(this Match match)
        {
            return match.Groups[match.Groups.Count - 1];
        }
    }
}

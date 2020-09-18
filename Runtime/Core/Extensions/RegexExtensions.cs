namespace Prateek.Runtime.Core.Extensions
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

namespace Prateek.Runtime.KeynameFramework
{
    using System.Collections.Generic;
    using System.Text;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.KeynameFramework.Enums;
    using Prateek.Runtime.KeynameFramework.Interfaces;
    using Prateek.Runtime.KeynameFramework.Settings;
    using UnityEngine;

    public static class KeynameExtensions
    {
        #region Class Methods
        /// <summary>
        ///     Adds the given keyword at the end of the keyname
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="keyword">The added keyword</param>
        public static void Add(ref this Keyname keyname, Keyword keyword)
        {
            keyname.keywords.Add(keyword);
            keyname.Invalidate();
        }

        /// <summary>
        ///     Adds the keyword of that type at the end of the keyname
        /// </summary>
        /// <typeparam name="TKeyword"></typeparam>
        /// <param name="keyname"></param>
        public static void Add<TKeyword>(ref this Keyname keyname)
            where TKeyword : MasterKeyword
        {
            keyname.Add(typeof(TKeyword));
        }

        /// <summary>
        ///     Remove the first keyword that matches
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="match">The matching keyword</param>
        public static void Remove(ref this Keyname keyname, Keyword match)
        {
            var index = keyname.FindIndex(match);
            if (index <= Const.INDEX_NONE)
            {
                return;
            }

            keyname.keywords.RemoveAt(index);
            keyname.Invalidate();
        }

        /// <summary>
        ///     Remove the first keyword that matches
        /// </summary>
        /// <param name="keyname">The target</param>
        public static void Remove<TKeyword>(ref this Keyname keyname)
            where TKeyword : IMasterMetaword
        {
            keyname.Remove(typeof(TKeyword));
        }

        /// <summary>
        ///     Remove all the keywords that match
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="match">The matching keyword</param>
        public static void RemoveAll(ref this Keyname keyname, Keyword match)
        {
            var index = 0;
            while (true)
            {
                index = keyname.FindIndex(match, index);
                if (index <= Const.INDEX_NONE)
                {
                    keyname.Invalidate();
                    return;
                }

                keyname.keywords.RemoveAt(index);
            }
        }

        /// <summary>
        ///     Remove all the keywords that match
        /// </summary>
        /// <param name="keyname">The target</param>
        public static void RemoveAll<TKeyword>(ref this Keyname keyname)
            where TKeyword : IMasterMetaword
        {
            keyname.RemoveAll(typeof(TKeyword));
        }

        /// <summary>
        ///     Insert given "insert" keyword before the given "match" keyword
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="insert">The Insert</param>
        /// <param name="match">The Match</param>
        public static void InsertBefore(ref this Keyname keyname, Keyword insert, Keyword match)
        {
            keyname.Insert(0, insert, match);
            keyname.Invalidate();
        }

        /// <summary>
        ///     Insert given "insert" keyword before the given Generic keyword
        /// </summary>
        /// <typeparam name="TKeyword"></typeparam>
        /// <param name="keyname">The target</param>
        /// <param name="insert">The Insert</param>
        public static void InsertBefore<TKeyword>(ref this Keyname keyname, Keyword insert)
            where TKeyword : IMasterMetaword
        {
            keyname.InsertBefore(insert, typeof(TKeyword));
        }

        /// <summary>
        ///     Insert given "insert" keyword after the given "match" keyword
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="insert">The Insert</param>
        /// <param name="match">The Match</param>
        public static void InsertAfter(ref this Keyname keyname, Keyword insert, Keyword match)
        {
            keyname.Insert(1, insert, match);
            keyname.Invalidate();
        }

        /// <summary>
        ///     Insert given "insert" keyword after the given "match" keyword
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="insert">The Insert</param>
        public static void InsertAfter<TKeyword>(ref this Keyname keyname, Keyword insert)
            where TKeyword : IMasterMetaword
        {
            keyname.InsertBefore(insert, typeof(TKeyword));
        }

        /// <summary>
        ///     Uses the given match to filter the matching keyword from the target keyname
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="match">The Match</param>
        /// <returns>The filtered keyname</returns>
        public static Keyname Filter(this Keyname keyname, Keyword match)
        {
            var result = new Keyname(false);
            result.settings = keyname.settings;
            for (var k = 0; k < keyname.keywords.Count; k++)
            {
                if (match.Match(keyname.keywords[k]))
                {
                    result.Add(keyname.keywords[k]);
                }
            }

            return result;
        }

        /// <summary>
        ///     Uses the generic type as a match to filter the matching keyword from the target keyname
        /// </summary>
        /// <typeparam name="TKeyword"></typeparam>
        /// <param name="keyname">The target</param>
        /// <returns>The filtered keyname</returns>
        public static Keyname Filter<TKeyword>(this Keyname keyname)
            where TKeyword : IMasterMetaword
        {
            return keyname.Filter(typeof(TKeyword));
        }

        /// <summary>
        /// Filter the given other keyname with the keywords in the calling one
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static Keyname Filter(this Keyname filter, Keyname other)
        {
            var result = new Keyname(false);
            result.settings = other.settings;
            foreach (var otherKeyword in other.keywords)
            {
                foreach (var filterKeyword in filter.keywords)
                {
                    if (filterKeyword.Match(otherKeyword))
                    {
                        result.Add(otherKeyword);
                        break;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Replaces the given match replace the first keyword found by the given replacement
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="replacement">The replacement</param>
        /// <param name="match">The Match</param>
        public static void Replace(ref this Keyname keyname, Keyword replacement, Keyword match)
        {
            var index = keyname.FindIndex(match);
            if (index <= Const.INDEX_NONE)
            {
                throw new KeyNotFoundException($"Keyword matching {match} could not be found");
            }

            keyname.keywords[index] = replacement;
            keyname.Invalidate();
        }

        /// <summary>
        ///     Replaces the given match replace the first keyword found by the given replacement
        /// </summary>
        /// <typeparam name="TKeyword"></typeparam>
        /// <param name="keyname">The target</param>
        /// <param name="replacement">The replacement</param>
        public static void Replace<TKeyword>(ref this Keyname keyname, Keyword replacement)
            where TKeyword : IMasterMetaword
        {
            keyname.Replace(replacement, typeof(TKeyword));
        }

        /// <summary>
        ///     Checks if the given keyword matches with any of the keyname ones
        /// </summary>
        /// <param name="keyname">The target</param>
        /// <param name="match">The Match</param>
        /// <returns></returns>
        public static bool Contains(this Keyname keyname, Keyword match)
        {
            return keyname.FindIndex(match) != Const.INDEX_NONE;
        }

        /// <summary>
        ///     Checks if the given generic keyword matches with any of the keyname ones
        /// </summary>
        /// <typeparam name="TKeyword"></typeparam>
        /// <param name="keyname"></param>
        /// <returns></returns>
        public static bool Contains<TKeyword>(this Keyname keyname)
            where TKeyword : IMasterMetaword
        {
            return keyname.Contains(typeof(TKeyword));
        }

        /// <summary>
        ///     Matches this UniqueId with the given other one
        /// </summary>
        /// <param name="matchee">The keyname analyzed</param>
        /// <returns>
        ///     See KeywordMatchResultType for the result of the match
        /// </returns>
        public static KeynameMatchResult Match(this Keyname matching, Keyname matchee)
        {
            var atLeastOneMatch = false;
            var resultType      = matching.keywords.Count == matchee.keywords.Count ? KeynameMatchType.Equal : KeynameMatchType.MatchFull;
            foreach (Keyword parent in matching.keywords)
            {
                var hasFound = false;
                foreach (Keyword child in matchee.keywords)
                {
                    if (parent.Match(child))
                    {
                        hasFound = true;
                        atLeastOneMatch = true;
                        break;
                    }
                }

                if (!hasFound)
                {
                    resultType = KeynameMatchType.MatchPartial;
                }
            }

            var tagCount = Mathf.Max(matching.keywords.Count, matchee.keywords.Count);
            if (!atLeastOneMatch && tagCount > 0)
            {
                resultType = KeynameMatchType.MatchFail;
            }

            return resultType;
        }

        private static void Insert(ref this Keyname keyname, int insertOffset, Keyword insert, Keyword match)
        {
            var index = keyname.FindIndex(match);
            if (index <= Const.INDEX_NONE)
            {
                throw new KeyNotFoundException($"Keyword matching {match} could not be found");
            }

            keyname.keywords.Insert(index + insertOffset, insert);
        }

        private static int FindIndex(this Keyname keyname, Keyword match, int startIndex = 0)
        {
            for (var k = startIndex; k < keyname.keywords.Count; k++)
            {
                if (match.Match(keyname.keywords[k]))
                {
                    return k;
                }
            }

            return Const.INDEX_NONE;
        }

        internal static void Invalidate(ref this Keyname keyname)
        {
            keyname.state = KeynameState.None;
            keyname.hash = 0;
            keyname.builtKeyname = null;
        }
        
        internal static void RebuildState(ref this Keyname keyname)
        {
            KeynameState state = KeynameState.Keywords;
            foreach (var keyword in keyname.keywords)
            {
                if (keyword.Status == KeywordStatus.Name)
                {
                    state = KeynameState.Fullname;
                    break;
                }
            }

            keyname.state = state;
        }

        internal static void GenerateHash(ref this Keyname keyname)
        {
            var hash = 1;
            foreach (var keyword in keyname.keywords)
            {
                hash ^= keyword.GetHashCode();
            }
            keyname.hash = hash;
        }

        internal static void BuildKeyname(ref this Keyname keyname)
        {
            var builder = new StringBuilder();
            var settings = keyname.settings == null ? KeynameSettings.Default.Data : keyname.settings;
            if (string.IsNullOrEmpty(settings.separatorForRebuild))
            {
                settings = null;
            }

            foreach (var keyword in keyname.keywords)
            {
                if (settings != null && builder.Length > 0)
                {
                    builder.Append(settings.separatorForRebuild);
                }

                builder.Append(keyword);
            }

            keyname.builtKeyname = builder.ToString();
        }
        #endregion
    }
}

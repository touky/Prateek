namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mayfair.Core.Code.Utils.Types;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Prateek.Core.Code.CachedArray;
    using UnityEngine;
    using UnityEngine.Assertions;

    public static class KeynameExtensions
    {

        /// <summary>
        ///     Matches this UniqueId with the given other one
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///     The result of the match:
        ///     Equal: Both UniqueIds are exactly equals
        ///     MatchFull: Left id is not a name, and all its tag match the right id
        ///     MatchPartial: Left id is not a name, and some its tag match the right id
        ///     MatchFail: No match whatsoever
        /// </returns>
        public static KeywordMatchResult Match(this Keyname keyname, Keyname other)
        {
            var tagResult = keyname.keywordArray.Match(other.keywordArray);
            switch ((KeywordMatchResultType) tagResult)
            {
                case KeywordMatchResultType.Equal:
                {
                    if (keyname.name == other.name)
                    {
                        return tagResult;
                    }
                    else if (keyname.Type == KeynameState.Fullname)
                    {
                        return KeywordMatchResultType.MatchFail;
                    }

                    return KeywordMatchResultType.MatchFull;
                }
                default:
                {
                    if (keyname.Type == KeynameState.Fullname)
                    {
                        return KeywordMatchResultType.MatchFail;
                    }

                    return tagResult;
                }
            }
        }

        /// <summary>
        ///     Uses this UniqueId as a filter to extract only the matching tags in the given one and return a filtered uniqueid
        ///     WARNING: this UniqueId MUST be a Type == UniqueIdType.Filter
        /// </summary>
        /// <param name="other">A uniqueId of filter type</param>
        /// <returns></returns>
        public static Keyname Filter(this Keyname keyname, Keyname other)
        {
            Assert.IsTrue(keyname.Type == KeynameState.Keywords);

            var result = new Keyname(false);
            result.AddTag(keyname.keywordArray.Filter(other.KeywordArray));
            return result;
        }

        public static void SetName(this Keyname keyname, string name = null)
        {
            keyname.name = string.IsNullOrEmpty(name) ? string.Empty : name;
        }

        internal static void AddTag(this Keyname keyname, KeywordArray container)
        {
            keyname.keywordArray.Add(container.Keywords);
            keyname.keyname = KeywordRegistry.ToString(keyname.keywordArray.Keywords);
        }

        public static void AddTag(this Keyname keyname, Type type)
        {
            if (KeywordRegistry.TagMatches(KeywordRegistry.RootTagType, type))
            {
                keyname.keywordArray.Add(new StaticArray10<Type> {type});
                keyname.keyname = KeywordRegistry.ToString(keyname.keywordArray.Keywords);
            }
            else
            {
                throw new ArgumentException(type.Name + " should be a " + typeof(MasterKeyword));
            }
        }
    }

    internal struct KeywordArray
    {
        #region Fields
        private CachedList10<Type> keywords;
        #endregion

        #region Properties
        public IReadOnlyList<Type> Keywords
        {
            get { return keywords; }
        }
        #endregion

        #region Constructors
        internal KeywordArray(bool empty)
        {
            keywords = new CachedList10<Type>();
        }

        internal KeywordArray(string source, out string name)
        {
            keywords = new CachedList10<Type>();
            name = string.Empty;// KeywordRegistry.Convert(source, out keywords);
        }
        #endregion

        #region Class Methods
        //Add / Remove / Insert / +-
        //SetNumber ?
        //SetName
        //Match / Filter / Replace
        //Create<T>
        //Create(Type)
        //[] Create<Interface>()

        internal void Add<T>() where T : MasterKeyword
        {
            Add(typeof(T));
        }
        
        internal void Add(Type type)
        {
            if (type == null || keywords.Contains(type))
            {
                return;
            }

            Assert.IsTrue(KeywordRegistry.TagMatches(KeywordRegistry.RootTagType, type));

            keywords.Add(KeywordRegistry.Get(type));
        }

        internal void Add(CachedList10<Type> tagsToAdd)
        {
            Add((IReadOnlyList<Type>)tagsToAdd);
        }
        
        internal void Add(IReadOnlyList<Type> tagsToAdd)
        {
            int remaining = CachedList10<Type>.SIZE - (keywords.Count + tagsToAdd.Count);
            if (remaining < 0)
            {
                Debug.LogError($"Filter only support {CachedList10<Type>.SIZE} tags. The last {Mathf.Abs(remaining)} field(s) will be omitted.");
            }

            int count = Mathf.Min(CachedList10<Type>.SIZE - keywords.Count, tagsToAdd.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(KeywordRegistry.TagMatches(KeywordRegistry.RootTagType, tagsToAdd[i]));

                keywords.Add(tagsToAdd[i]);
            }
        }

        internal void Remove<T>() where T : MasterKeyword
        {
            Remove(typeof(T));
        }

        internal void Remove(Type type)
        {
            Assert.IsTrue(KeywordRegistry.TagMatches(KeywordRegistry.RootTagType, type));
            for (int k = 0; k < keywords.Count; k++)
            {
                var keyword = keywords[k];
                if (!KeywordRegistry.TagMatches(type, keyword))
                {
                    continue;
                }

                keywords.RemoveAt(k--);
            }
        }

        internal KeywordMatchResult Match(KeywordArray other)
        {
            bool atLeastOneMatch = false;
            KeywordMatchResultType resultType = keywords.Count == other.keywords.Count ? KeywordMatchResultType.Equal : KeywordMatchResultType.MatchFull;
            foreach (Type source in keywords)
            {
                bool hasFound = false;
                foreach (Type otherTag in other.keywords)
                {
                    if (KeywordRegistry.TagMatches(source, otherTag))
                    {
                        hasFound = true;
                        atLeastOneMatch = true;
                        break;
                    }
                }

                if (!hasFound)
                {
                    resultType = KeywordMatchResultType.MatchPartial;
                }
            }

            int tagCount = Mathf.Max(keywords.Count, other.keywords.Count);
            if (!atLeastOneMatch && tagCount > 0)
            {
                resultType = KeywordMatchResultType.MatchFail;
            }

            return resultType;
        }

        internal KeywordArray Filter(KeywordArray other)
        {
            KeywordArray result = new KeywordArray(false);
            foreach (Type otherTag in other.Keywords)
            {
                foreach (Type filterTag in keywords)
                {
                    if (!KeywordRegistry.TagMatches(filterTag, otherTag))
                    {
                        continue;
                    }

                    result.keywords.Add(otherTag);
                    break;
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            int hash = 1;
            foreach (object tag in keywords)
            {
                hash ^= tag.GetHashCode();
            }

            return hash;
        }

        public override string ToString()
        {
            bool hasSeveral = false;
            StringBuilder builder = new StringBuilder("<");
            foreach (Type type in Keywords)
            {
                if (hasSeveral)
                {
                    builder.Append("/");
                }

                builder.Append($"{type.Name}");
                hasSeveral = true;
            }

            builder.Append(">");
            return builder.ToString();
        }
        #endregion
    }
}

namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mayfair.Core.Code.Utils.Types;
    using UnityEngine;
    using UnityEngine.Assertions;

    internal struct KeywordHolder
    {
        #region Fields
        private StaticArray10<Type> keywords;
        #endregion

        #region Properties
        public IReadOnlyList<Type> Keywords
        {
            get { return keywords; }
        }
        #endregion

        #region Constructors
        internal KeywordHolder(bool empty)
        {
            keywords = new StaticArray10<Type>();
        }

        internal KeywordHolder(string source, out string name)
        {
            name = KeywordBank.Convert(source, out keywords);
        }
        #endregion

        #region Class Methods
        internal void Add<T>() where T : MasterKeyword
        {
            Type type = typeof(T);
            if (keywords.Contains(type))
            {
                return;
            }

            Assert.IsTrue(KeywordBank.TagMatches(KeywordBank.RootTagType, type));

            keywords.Add(KeywordBank.Get(type));
        }
        
        internal void Add(Type type)
        {
            if (type == null)
            {
                return;
            }

            Assert.IsTrue(KeywordBank.TagMatches(KeywordBank.RootTagType, type));

            keywords.Add(KeywordBank.Get(type));
        }

        internal void Add(StaticArray10<Type> tagsToAdd)
        {
            Add((IReadOnlyList<Type>)tagsToAdd);
        }

        internal void Add(IReadOnlyList<Type> tagsToAdd)
        {
            int remaining = StaticArray10<Type>.SIZE - (keywords.Count + tagsToAdd.Count);
            if (remaining < 0)
            {
                Debug.LogError($"Filter only support {StaticArray10<Type>.SIZE} tags. The last {Mathf.Abs(remaining)} field(s) will be omitted.");
            }

            int count = Mathf.Min(StaticArray10<Type>.SIZE - keywords.Count, tagsToAdd.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(KeywordBank.TagMatches(KeywordBank.RootTagType, tagsToAdd[i]));

                keywords.Add(tagsToAdd[i]);
            }
        }

        internal KeywordMatchResult Match(KeywordHolder other)
        {
            bool atLeastOneMatch = false;
            KeywordMatchResultType resultType = keywords.Count == other.keywords.Count ? KeywordMatchResultType.Equal : KeywordMatchResultType.MatchFull;
            foreach (Type source in keywords)
            {
                bool hasFound = false;
                foreach (Type otherTag in other.keywords)
                {
                    if (KeywordBank.TagMatches(source, otherTag))
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

            if (!atLeastOneMatch)
            {
                resultType = KeywordMatchResultType.MatchFail;
            }

            return resultType;
        }

        internal KeywordHolder Filter(KeywordHolder other)
        {
            KeywordHolder result = new KeywordHolder(false);
            foreach (Type otherTag in other.Keywords)
            {
                foreach (Type filterTag in keywords)
                {
                    if (!KeywordBank.TagMatches(filterTag, otherTag))
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

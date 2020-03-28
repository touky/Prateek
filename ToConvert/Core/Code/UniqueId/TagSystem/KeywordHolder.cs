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
            name = TagBank.Convert(source, out keywords);
        }
        #endregion

        #region Class Methods
        public void Add<T>() where T : MasterTag
        {
            Type type = typeof(T);
            if (keywords.Contains(type))
            {
                return;
            }

            Assert.IsTrue(TagBank.TagMatches(TagBank.RootTagType, type));

            keywords.Add(type);
        }
        
        public void Add(Type type)
        {
            Assert.IsTrue(TagBank.TagMatches(TagBank.RootTagType, type));

            keywords.Add(type);
        }

        public void Add(StaticArray10<Type> tagsToAdd)
        {
            Add((IReadOnlyList<Type>)tagsToAdd);
        }

        public void Add(IReadOnlyList<Type> tagsToAdd)
        {
            int remaining = StaticArray10<Type>.SIZE - (keywords.Count + tagsToAdd.Count);
            if (remaining < 0)
            {
                Debug.LogError($"Filter only support {StaticArray10<Type>.SIZE} tags. The last {Mathf.Abs(remaining)} field(s) will be omitted.");
            }

            int count = Mathf.Min(StaticArray10<Type>.SIZE - keywords.Count, tagsToAdd.Count);
            for (int i = 0; i < count; i++)
            {
                Assert.IsTrue(TagBank.TagMatches(TagBank.RootTagType, tagsToAdd[i]));

                keywords.Add(tagsToAdd[i]);
            }
        }

        public TagMatchResult Match(KeywordHolder other)
        {
            bool atLeastOneMatch = false;
            TagMatchResultType result = keywords.Count == other.keywords.Count ? TagMatchResultType.Equal : TagMatchResultType.MatchFull;
            foreach (Type source in keywords)
            {
                bool hasFound = false;
                foreach (Type otherTag in other.keywords)
                {
                    if (TagBank.TagMatches(source, otherTag))
                    {
                        hasFound = true;
                        atLeastOneMatch = true;
                        break;
                    }
                }

                if (!hasFound)
                {
                    result = TagMatchResultType.MatchPartial;
                }
            }

            if (!atLeastOneMatch)
            {
                result = TagMatchResultType.MatchFail;
            }

            return result;
        }

        public KeywordHolder Filter(KeywordHolder other)
        {
            KeywordHolder result = new KeywordHolder(false);
            foreach (Type otherTag in other.Keywords)
            {
                foreach (Type filterTag in keywords)
                {
                    if (!TagBank.TagMatches(filterTag, otherTag))
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

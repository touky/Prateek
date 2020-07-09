namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Mayfair.Core.Code.Utils.Types;
    using UnityEngine;
    using UnityEngine.Assertions;

    internal struct KeywordArray
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
        internal KeywordArray(bool empty)
        {
            keywords = new StaticArray10<Type>();
        }

        internal KeywordArray(string source, out string name)
        {
            name = KeywordRegistry.Convert(source, out keywords);
        }
        #endregion

        #region Class Methods
        //Add / Remove / Insert
        //SetNumber ?
        //SetName
        //Match / Filter
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

            if (!atLeastOneMatch)
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

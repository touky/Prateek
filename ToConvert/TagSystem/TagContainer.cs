namespace Assets.Prateek.ToConvert.TagSystem
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Assets.Prateek.ToConvert.CachedArray;
    using UnityEngine;
    using UnityEngine.Assertions;

    public struct TagContainer
    {
        #region Fields
        private StaticArray10<Type> tags;
        #endregion

        #region Properties
        public IReadOnlyList<Type> Tags
        {
            get { return tags; }
        }
        #endregion

        #region Constructors
        //TODO: benjaminh: This is *HIGHLY* temporary
        private TagContainer(bool empty)
        {
            tags = new StaticArray10<Type>();
        }

        public TagContainer(string source, out string name)
        {
            name = TagBank.Convert(source, out tags);
        }
        #endregion

        #region Class Methods
        public static TagContainer Create<T>()
            where T : MasterTag
        {
            var container = new TagContainer(true);
            container.Add<T>();
            return container;
        }

        public static TagContainer Create<T0, T1>()
            where T0 : MasterTag
            where T1 : MasterTag
        {
            var container = Create<T0>();
            container.Add<T1>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
        {
            var container = Create<T0, T1>();
            container.Add<T2>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
        {
            var container = Create<T0, T1, T2>();
            container.Add<T3>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3, T4>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
        {
            var container = Create<T0, T1, T2, T3>();
            container.Add<T4>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3, T4, T5>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
        {
            var container = Create<T0, T1, T2, T3, T4>();
            container.Add<T5>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3, T4, T5, T6>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
        {
            var container = Create<T0, T1, T2, T3, T4, T5>();
            container.Add<T6>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3, T4, T5, T6, T7>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
        {
            var container = Create<T0, T1, T2, T3, T4, T5, T6>();
            container.Add<T7>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
            where T8 : MasterTag
        {
            var container = Create<T0, T1, T2, T3, T4, T5, T6, T7>();
            container.Add<T8>();
            return container;
        }

        public static TagContainer Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
            where T8 : MasterTag
            where T9 : MasterTag
        {
            var container = Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            container.Add<T9>();
            return container;
        }

        public void Add<T>() where T : MasterTag
        {
            var type = typeof(T);
            if (tags.Contains(type))
            {
                return;
            }

            Assert.IsTrue(TagMatches(typeof(MasterTag), type));

            tags.Add(type);
        }

        public void Add(Type type)
        {
            Assert.IsTrue(TagMatches(typeof(MasterTag), type));

            tags.Add(type);
        }

        public void Add(StaticArray10<Type> tagsToAdd)
        {
            Add((IReadOnlyList<Type>) tagsToAdd);
        }

        public void Add(IReadOnlyList<Type> tagsToAdd)
        {
            var remaining = StaticArray10<Type>.SIZE - (tags.Count + tagsToAdd.Count);
            if (remaining < 0)
            {
                //DebugTools.LogError($"Filter only support 10 tags. The last {Mathf.Abs(remaining)} field(s) will be omitted.");
            }

            var count = Mathf.Min(StaticArray10<Type>.SIZE - tags.Count, tagsToAdd.Count);
            for (var i = 0; i < count; i++)
            {
                Assert.IsTrue(TagMatches(typeof(MasterTag), tagsToAdd[i]));

                tags.Add(tagsToAdd[i]);
            }
        }

        public static bool TagMatches(Type child, Type parent)
        {
            return parent == child || parent.IsSubclassOf(child);
        }

        public TagMatchResult Match(TagContainer other)
        {
            var atLeastOneMatch = false;
            var result          = tags.Count == other.tags.Count ? TagMatchResultType.Equal : TagMatchResultType.MatchFull;
            foreach (Type source in tags)
            {
                var hasFound = false;
                foreach (Type otherTag in other.tags)
                {
                    if (TagMatches(source, otherTag))
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

        public TagContainer Filter(TagContainer other)
        {
            var result = new TagContainer(false);
            foreach (var otherTag in other.Tags)
            {
                foreach (Type filterTag in tags)
                {
                    if (!TagMatches(filterTag, otherTag))
                    {
                        continue;
                    }

                    result.tags.Add(otherTag);
                    break;
                }
            }

            return result;
        }

        public override int GetHashCode()
        {
            var hash = 1;
            foreach (var tag in tags)
            {
                hash ^= tag.GetHashCode();
            }

            return hash;
        }

        public override string ToString()
        {
            var hasSeveral = false;
            var builder    = new StringBuilder("<");
            foreach (var type in Tags)
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

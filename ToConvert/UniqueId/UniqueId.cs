namespace Assets.Prateek.ToConvert.UniqueId
{
    using System;
    using System.Diagnostics;
    using Assets.Prateek.ToConvert.CachedArray;
    using Assets.Prateek.ToConvert.TagSystem;
    using UnityEngine.Assertions;

    [DebuggerDisplay("UniqueId:{uniqueId}/{GetHexHashCode()}")]
    public struct UniqueId : IEquatable<UniqueId>
    {
        #region Fields
        private string uniqueId;
        #endregion

        private string name;
        private TagContainer tagContainer;

        #region Properties
        //Todo: benjaminh: remvoe this
        public string RawValue
        {
            get { return uniqueId; }
        }

        public UniqueIdType Type
        {
            get
            {
                if (!string.IsNullOrEmpty(name))
                {
                    return UniqueIdType.Name;
                }

                return tagContainer.Tags.Count > 0 ? UniqueIdType.Filter : UniqueIdType.None;
            }
        }

        public TagContainer TagContainer
        {
            get { return tagContainer; }
        }
        #endregion

        #region Constructors
        public UniqueId(string uniqueId)
        {
            Assert.IsNotNull(uniqueId);

            this.uniqueId = string.IsNullOrEmpty(uniqueId) ? string.Empty : uniqueId;
            tagContainer = new TagContainer(this.uniqueId, out name);
        }

        public UniqueId(Type type)
        {
            Assert.IsNotNull(type);
            Assert.IsTrue(TagContainer.TagMatches(typeof(MasterTag), type));

            tagContainer = new TagContainer();
            tagContainer.Add(type);

            name = string.Empty;
            uniqueId = type.Name;
        }

        public UniqueId(Type type0, Type type1)
        {
            Assert.IsNotNull(type1);
            Assert.IsTrue(TagContainer.TagMatches(typeof(MasterTag), type1));

            tagContainer = new TagContainer();
            tagContainer.Add(type0);
            tagContainer.Add(type1);

            name = string.Empty;
            uniqueId = TagBank.ToName(tagContainer.Tags);
        }

        private UniqueId(bool noCheck)
        {
            uniqueId = string.Empty;
            name = string.Empty;
            tagContainer = new TagContainer();
        }

        public static UniqueId Create<T>(string name = null)
            where T : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T>();
            return id;
        }

        public static UniqueId Create<T0, T1>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3, T4>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3, T4>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3, T4>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3, T4, T5>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3, T4, T5>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3, T4, T5>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3, T4, T5, T6>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3, T4, T5, T6>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3, T4, T5, T6>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3, T4, T5, T6, T7>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
        {
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3, T4, T5, T6, T7>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3, T4, T5, T6, T7>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string name = null)
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
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3, T4, T5, T6, T7, T8>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            return id;
        }

        public static UniqueId Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name = null)
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
            var id = new UniqueId(true);
            id.uniqueId = TagBank.ToName<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(name);
            id.name = string.IsNullOrEmpty(name) ? string.Empty : name;
            id.tagContainer = TagContainer.Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            return id;
        }
        #endregion

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
        public TagMatchResult Match(UniqueId other)
        {
            var tagResult = tagContainer.Match(other.tagContainer);
            switch ((TagMatchResultType) tagResult)
            {
                case TagMatchResultType.Equal:
                {
                    if (name == other.name)
                    {
                        return tagResult;
                    }
                    else
                    {
                        return TagMatchResultType.MatchFull;
                    }
                }
                default:
                {
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
        public UniqueId Filter(UniqueId other)
        {
            Assert.IsTrue(Type == UniqueIdType.Filter);

            var result = new UniqueId(false);
            result.AddTags(tagContainer.Filter(other.TagContainer));
            return result;
        }

        #region Class Methods
        public static bool TryGetTag(UniqueId uniqueId, out Type tagFound)
        {
            tagFound = TagBank.Get(uniqueId);
            return TagContainer.TagMatches(typeof(MasterTag), tagFound);
        }

        public static void ExtractTags(string source, out StaticArray10<Type> tags)
        {
            TagBank.Convert(source, out tags);
        }

        public static implicit operator UniqueId(string uniqueId)
        {
            return new UniqueId(uniqueId);
        }

        public static implicit operator string(UniqueId uniqueId)
        {
            return uniqueId.uniqueId;
        }

        public static bool operator ==(UniqueId a, UniqueId b)
        {
            return a.uniqueId == b.uniqueId;
        }

        public static bool operator !=(UniqueId a, UniqueId b)
        {
            return a.uniqueId != b.uniqueId;
        }

        public void AddTags(string source)
        {
            ExtractTags(source, out var tagsExtracted);
            //DebugTools.Log("tagsExtracted count: " + tagsExtracted.Count);
            tagContainer.Add(tagsExtracted);
            uniqueId = TagBank.ToName(tagContainer.Tags);
        }

        private void AddTags(TagContainer container)
        {
            tagContainer.Add(container.Tags);
            uniqueId = TagBank.ToName(tagContainer.Tags);
        }

        public void AddTag(Type type)
        {
            if (TagContainer.TagMatches(typeof(MasterTag), type))
            {
                tagContainer.Add(new StaticArray10<Type> {type});
                uniqueId = TagBank.ToName(tagContainer.Tags);
            }
            else
            {
                throw new ArgumentException(type.Name + " should be a " + typeof(MasterTag));
            }
        }

        public override bool Equals(object other)
        {
            if (!(other is UniqueId))
            {
                return false;
            }

            return this == (UniqueId) other;
        }

        public override int GetHashCode()
        {
            var hash = 1;
            if (uniqueId.Length != 0)
            {
                hash ^= uniqueId.GetHashCode();
            }

            return hash;
        }

        private string GetHexHashCode()
        {
            return string.Format("{0:X}", GetHashCode());
        }

        public override string ToString()
        {
            return RawValue;
        }
        #endregion

        #region IEquatable<UniqueId> Members
        public bool Equals(UniqueId other)
        {
            return other == this;
        }
        #endregion
    }
}

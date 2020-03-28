namespace Mayfair.Core.Code.Utils.Types.UniqueId
{
    using System;
    using System.Diagnostics;
    using Mayfair.Core.Code.TagSystem;
    using UnityEngine.Assertions;

    [DebuggerDisplay("Keyname:{keyname}/{GetHexHashCode()}")]
    public struct Keyname : IEquatable<Keyname>
    {
        #region Fields
        private string keyname;

        private string name;
        private KeywordHolder keywordHolder;
        #endregion

        #region Properties
        //Todo: benjaminh: remvoe this
        public string RawValue
        {
            get { return keyname; }
        }

        public KeynameState Type
        {
            get
            {
                if (!string.IsNullOrEmpty(name))
                {
                    return KeynameState.Fullname;
                }

                return keywordHolder.Keywords.Count > 0 ? KeynameState.Keywords : KeynameState.None;
            }
        }

        internal KeywordHolder KeywordHolder
        {
            get { return keywordHolder; }
        }
        #endregion

        #region Constructors
        private Keyname(bool noCheck)
        {
            keyname = string.Empty;
            name = string.Empty;
            keywordHolder = new KeywordHolder();
        }
        #endregion

        #region Static Constructors
        public static Keyname Create(string uniqueId)
        {
            Assert.IsNotNull(uniqueId);

            var id   = new Keyname(true);
            var name = string.Empty;
            id.keywordHolder = new KeywordHolder(uniqueId, out id.name);
            id.name = name;
            id.keyname = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, string name = null)
        {
            var id = new Keyname(true);
            id.keywordHolder = new KeywordHolder(false);
            id.keywordHolder.Add(type0);
            id.SetName(name);
            id.keyname = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, string name = null)
        {
            var id = Create(type0, name);
            id.keywordHolder.Add(type1);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, string name = null)
        {
            var id = Create(type0, type1, name);
            id.keywordHolder.Add(type2);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, string name = null)
        {
            var id = Create(type0, type1, type2, name);
            id.keywordHolder.Add(type3);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, string name = null)
        {
            var id = Create(type0, type1, type2, type3, name);
            id.keywordHolder.Add(type4);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, string name = null)
        {
            var id = Create(type0, type1, type2, type3, type4, name);
            id.keywordHolder.Add(type5);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, string name = null)
        {
            var id = Create(type0, type1, type2, type3, type4, type5, name);
            id.keywordHolder.Add(type6);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, string name = null)
        {
            var id = Create(type0, type1, type2, type3, type4, type5, type6, name);
            id.keywordHolder.Add(type7);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8, string name = null)
        {
            var id = Create(type0, type1, type2, type3, type4, type5, type6, type7, name);
            id.keywordHolder.Add(type8);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8, Type type9, string name = null)
        {
            var id = Create(type0, type1, type2, type3, type4, type5, type6, type7, type8, name);
            id.keywordHolder.Add(type9);
            id = TagBank.ToString(id.keywordHolder.Keywords, name);
            return id;
        }

        public static Keyname Create<T0>(string name = null)
            where T0 : MasterTag
        {
            return Create(typeof(T0), name);
        }

        public static Keyname Create<T0, T1>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), name);
        }

        public static Keyname Create<T0, T1, T2>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), name);
        }

        public static Keyname Create<T0, T1, T2, T3>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7>(string name = null)
            where T0 : MasterTag
            where T1 : MasterTag
            where T2 : MasterTag
            where T3 : MasterTag
            where T4 : MasterTag
            where T5 : MasterTag
            where T6 : MasterTag
            where T7 : MasterTag
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string name = null)
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
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T8), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name = null)
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
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T8), typeof(T9), name);
        }
        #endregion Static Constructors

        #region Class Methods
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
        public TagMatchResult Match(Keyname other)
        {
            var tagResult = keywordHolder.Match(other.keywordHolder);
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
        public Keyname Filter(Keyname other)
        {
            Assert.IsTrue(Type == KeynameState.Keywords);

            var result = new Keyname(false);
            result.AddTag(keywordHolder.Filter(other.KeywordHolder));
            return result;
        }
                
        public void SetName(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? string.Empty : name;
        }
        
        private void AddTag(KeywordHolder container)
        {
            keywordHolder.Add(container.Keywords);
            keyname = TagBank.ToString(keywordHolder.Keywords);
        }

        public void AddTag(Type type)
        {
            if (TagBank.TagMatches(TagBank.RootTagType, type))
            {
                keywordHolder.Add(new StaticArray10<Type> {type});
                keyname = TagBank.ToString(keywordHolder.Keywords);
            }
            else
            {
                throw new ArgumentException(type.Name + " should be a " + typeof(MasterTag));
            }
        }
        #endregion

        #region operators
        public static implicit operator Keyname(string uniqueId)
        {
            return Create(uniqueId);
        }

        public static implicit operator string(Keyname keyname)
        {
            return keyname.keyname;
        }

        public static bool operator ==(Keyname a, Keyname b)
        {
            return a.keyname == b.keyname;
        }

        public static bool operator !=(Keyname a, Keyname b)
        {
            return a.keyname != b.keyname;
        }

        public override bool Equals(object other)
        {
            if (!(other is Keyname))
            {
                return false;
            }

            return this == (Keyname) other;
        }

        public override int GetHashCode()
        {
            var hash = 1;
            if (keyname.Length != 0)
            {
                hash ^= keyname.GetHashCode();
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
        public bool Equals(Keyname other)
        {
            return other == this;
        }
        #endregion
    }
}

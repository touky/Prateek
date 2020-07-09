namespace Mayfair.Core.Code.Utils.Types.UniqueId
{
    using System;
    using System.Diagnostics;
    using Mayfair.Core.Code.TagSystem;
    using UnityEngine.Assertions;

    [DebuggerDisplay("Keyname:{keyname}/{GetHexHashCode()}")]
    public struct Keyname : IEquatable<Keyname>
    {
        //Add / Remove / Insert
        //SetNumber ?
        //SetName
        //Match / Filter
        //Create<T>
        //Create(Type)
        //[] Create<Interface>()

        #region Fields
        private string keyname;

        private string name;
        private KeywordArray keywordArray;
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

                return keywordArray.Keywords.Count > 0 ? KeynameState.Keywords : KeynameState.None;
            }
        }

        internal KeywordArray KeywordArray
        {
            get { return keywordArray; }
        }
        #endregion

        #region Constructors
        private Keyname(bool noCheck)
        {
            keyname = string.Empty;
            name = string.Empty;
            keywordArray = new KeywordArray();
        }
        #endregion

        #region Static Constructors
        public static Keyname Create(string stringId)
        {
            Assert.IsNotNull(stringId);

            var id   = new Keyname(true);
            var name = string.Empty;
            id.keywordArray = new KeywordArray(stringId, out id.name);
            id.name = name;
            id.keyname = KeywordRegistry.ToString(id.keywordArray.Keywords, name);
            return id;
        }

        public static Keyname Create(Type type0, string name = null)
        {
            return Create(type0, null, null, null, null, null, null, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, string name = null)
        {
            return Create(type0, type1, null, null, null, null, null, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, string name = null)
        {
            return Create(type0, type1, type2, null, null, null, null, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, string name = null)
        {
            return Create(type0, type1, type2, type3, null, null, null, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, string name = null)
        {
            return Create(type0, type1, type2, type3, type4, null, null, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, string name = null)
        {
            return Create(type0, type1, type2, type3, type4, type5, null, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, string name = null)
        {
            return Create(type0, type1, type2, type3, type4, type5, type6, null, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, string name = null)
        {
            return Create(type0, type1, type2, type3, type4, type5, type6, type7, null, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8, string name = null)
        {
            return Create(type0, type1, type2, type3, type4, type5, type6, type7, type8, null, name);
        }

        public static Keyname Create(Type type0, Type type1, Type type2, Type type3, Type type4, Type type5, Type type6, Type type7, Type type8, Type type9, string name = null)
        {
            var id = new Keyname(true);
            id.SetName(name);
            id.keywordArray = new KeywordArray(false);
            id.keywordArray.Add(type0);
            id.keywordArray.Add(type1);
            id.keywordArray.Add(type2);
            id.keywordArray.Add(type3);
            id.keywordArray.Add(type4);
            id.keywordArray.Add(type5);
            id.keywordArray.Add(type6);
            id.keywordArray.Add(type7);
            id.keywordArray.Add(type8);
            id.keywordArray.Add(type9);
            id.keyname = KeywordRegistry.ToString(id.keywordArray.Keywords, name);
            return id;
        }

        public static Keyname Create<T0>(string name = null)
            where T0 : MasterKeyword
        {
            return Create(typeof(T0), name);
        }

        public static Keyname Create<T0, T1>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), name);
        }

        public static Keyname Create<T0, T1, T2>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), name);
        }

        public static Keyname Create<T0, T1, T2, T3>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
            where T5 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
            where T5 : MasterKeyword
            where T6 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
            where T5 : MasterKeyword
            where T6 : MasterKeyword
            where T7 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
            where T5 : MasterKeyword
            where T6 : MasterKeyword
            where T7 : MasterKeyword
            where T8 : MasterKeyword
        {
            return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T8), name);
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
            where T5 : MasterKeyword
            where T6 : MasterKeyword
            where T7 : MasterKeyword
            where T8 : MasterKeyword
            where T9 : MasterKeyword
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
        public KeywordMatchResult Match(Keyname other)
        {
            var tagResult = keywordArray.Match(other.keywordArray);
            switch ((KeywordMatchResultType) tagResult)
            {
                case KeywordMatchResultType.Equal:
                {
                    if (name == other.name)
                    {
                        return tagResult;
                    }
                    else if (Type == KeynameState.Fullname)
                    {
                        return KeywordMatchResultType.MatchFail;
                    }

                    return KeywordMatchResultType.MatchFull;
                }
                default:
                {
                    if (Type == KeynameState.Fullname)
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
        public Keyname Filter(Keyname other)
        {
            Assert.IsTrue(Type == KeynameState.Keywords);

            var result = new Keyname(false);
            result.AddTag(keywordArray.Filter(other.KeywordArray));
            return result;
        }

        public void SetName(string name = null)
        {
            name = string.IsNullOrEmpty(name) ? string.Empty : name;
        }

        private void AddTag(KeywordArray container)
        {
            keywordArray.Add(container.Keywords);
            keyname = KeywordRegistry.ToString(keywordArray.Keywords);
        }

        public void AddTag(Type type)
        {
            if (KeywordRegistry.TagMatches(KeywordRegistry.RootTagType, type))
            {
                keywordArray.Add(new StaticArray10<Type> {type});
                keyname = KeywordRegistry.ToString(keywordArray.Keywords);
            }
            else
            {
                throw new ArgumentException(type.Name + " should be a " + typeof(MasterKeyword));
            }
        }
        #endregion

        #region operators
        public static implicit operator Keyname(string keyname)
        {
            return Create(keyname);
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

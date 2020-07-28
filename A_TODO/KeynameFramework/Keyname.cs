namespace Mayfair.Core.Code.Utils.Types.UniqueId
{
    using System;
    using System.Diagnostics;
    using Mayfair.Core.Code.TagSystem;
    using Prateek.Core.Code.CachedArray;
    using UnityEngine.Assertions;

    [DebuggerDisplay("Keyname:{keyname}/{GetHexHashCode()}")]
    public struct Keyname : IEquatable<Keyname>
    {
        //Add / Remove / Insert / +-
        //SetNumber ?
        //SetName
        //Match / Filter / Replace
        //Create<T>
        //Create(Type)
        //[] Create<Interface>()

        #region Fields
        internal CachedList10<Type> keywords;

        internal string keyname;
        internal string name;
        internal KeywordArray keywordArray;
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
        internal Keyname(bool noCheck)
        {
            keywords = new CachedList10<Type>();

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

        //public static Keyname Create<T0>(string name = null)
        //    where T0 : MasterKeyword
        //{
        //    return Create(typeof(T0), name);
        //}

        //public static Keyname Create<T0, T1>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), name);
        //}

        //public static Keyname Create<T0, T1, T2>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3, T4>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //    where T4 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3, T4, T5>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //    where T4 : MasterKeyword
        //    where T5 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3, T4, T5, T6>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //    where T4 : MasterKeyword
        //    where T5 : MasterKeyword
        //    where T6 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //    where T4 : MasterKeyword
        //    where T5 : MasterKeyword
        //    where T6 : MasterKeyword
        //    where T7 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //    where T4 : MasterKeyword
        //    where T5 : MasterKeyword
        //    where T6 : MasterKeyword
        //    where T7 : MasterKeyword
        //    where T8 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T8), name);
        //}

        //public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name = null)
        //    where T0 : MasterKeyword
        //    where T1 : MasterKeyword
        //    where T2 : MasterKeyword
        //    where T3 : MasterKeyword
        //    where T4 : MasterKeyword
        //    where T5 : MasterKeyword
        //    where T6 : MasterKeyword
        //    where T7 : MasterKeyword
        //    where T8 : MasterKeyword
        //    where T9 : MasterKeyword
        //{
        //    return Create(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T8), typeof(T9), name);
        //}
        #endregion Static Constructors

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

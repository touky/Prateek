namespace Prateek.KeynameFramework
{
    using System;
    using System.Diagnostics;
    using System.Text;
    using Prateek.Core.Code.CachedList;
    using Prateek.KeynameFramework.Enums;
    using Prateek.KeynameFramework.Interfaces;
    using UnityEngine.Assertions;

    [DebuggerDisplay("Keyname: {DebugString()}/{IntExtensions.ToHex(hash)}")]
    public struct Keyname : IEquatable<Keyname>
    {
        #region Fields
        internal KeynameSettingsData settings;
        internal CachedList10<Keyword> keywords;
        internal KeynameState state;
        internal int hash;
        internal string builtKeyname;
        #endregion

        #region Properties
        public KeynameState State
        {
            get
            {
                if (state == KeynameState.None)
                {
                    this.RebuildState();
                }

                return state;
            }
        }
        #endregion

        #region Constructors
        internal Keyname(bool noCheck)
        {
            settings = null;
            keywords = new CachedList10<Keyword>();
            state = KeynameState.None;
            hash = 0;
            builtKeyname = null;
        }
        #endregion

        #region Class Methods
        public static Keyname Create(string stringId, KeynameSettingsData settings = null)
        {
            Assert.IsNotNull(stringId);

            return KeywordRegistry.Convert(stringId, settings);
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
            if (hash == 0)
            {
                this.GenerateHash();
            }

            return hash;
        }

        public override string ToString()
        {
            if (builtKeyname == null)
            {
                this.BuildKeyname();
            }

            return builtKeyname;
        }

        public string DebugString()
        {
            var builder = new StringBuilder();
            foreach (var keyword in keywords)
            {
                builder.Append(keyword.DebugString());
            }

            return builder.ToString();
        }
        #endregion

        #region IEquatable<Keyname> Members
        public bool Equals(Keyname other)
        {
            return this.Match(other) == KeynameMatchType.Equal;
        }
        #endregion

        #region Keyword Create
        public static Keyname Create(Keyword k0)
        {
            var keyname = new Keyname(false);
            keyname.Add(k0);
            return keyname;
        }

        public static Keyname Create(Keyword k0, Keyword k1)
        {
            return Create(k0) + k1;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2)
        {
            return Create(k0, k1) + k2;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3)
        {
            return Create(k0, k1, k2) + k3;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3, Keyword k4)
        {
            return Create(k0, k1, k2, k3) + k4;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3, Keyword k4, Keyword k5)
        {
            return Create(k0, k1, k2, k3, k4) + k5;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3, Keyword k4, Keyword k5, Keyword k6)
        {
            return Create(k0, k1, k2, k3, k4, k5) + k6;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3, Keyword k4, Keyword k5, Keyword k6, Keyword k7)
        {
            return Create(k0, k1, k2, k3, k4, k5, k6) + k7;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3, Keyword k4, Keyword k5, Keyword k6, Keyword k7, Keyword k8)
        {
            return Create(k0, k1, k2, k3, k4, k5, k6, k7) + k8;
        }

        public static Keyname Create(Keyword k0, Keyword k1, Keyword k2, Keyword k3, Keyword k4, Keyword k5, Keyword k6, Keyword k7, Keyword k8, Keyword k9)
        {
            return Create(k0, k1, k2, k3, k4, k5, k6, k7, k8) + k9;
        }
        #endregion

        #region Typed Create
        public static Keyname Create<T0>(string name = null)
            where T0 : MasterKeyword
        {
            var keyname = new Keyname(false);
            keyname.Add<T0>();
            if (name != null)
            {
                keyname.Add(name);
            }

            return keyname;
        }

        public static Keyname Create<T0, T1>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
        {
            var keyname = Create<T0>(name);
            keyname.Add<T1>();
            return keyname;
        }

        public static Keyname Create<T0, T1, T2>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
        {
            var keyname = Create<T0, T1>(name);
            keyname.Add<T2>();
            return keyname;
        }

        public static Keyname Create<T0, T1, T2, T3>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
        {
            var keyname = Create<T0, T1, T2>(name);
            keyname.Add<T3>();
            return keyname;
        }

        public static Keyname Create<T0, T1, T2, T3, T4>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
        {
            var keyname = Create<T0, T1, T2, T3>(name);
            keyname.Add<T4>();
            return keyname;
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5>(string name = null)
            where T0 : MasterKeyword
            where T1 : MasterKeyword
            where T2 : MasterKeyword
            where T3 : MasterKeyword
            where T4 : MasterKeyword
            where T5 : MasterKeyword
        {
            var keyname = Create<T0, T1, T2, T3, T4>(name);
            keyname.Add<T5>();
            return keyname;
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
            var keyname = Create<T0, T1, T2, T3, T4, T5>(name);
            keyname.Add<T6>();
            return keyname;
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
            var keyname = Create<T0, T1, T2, T3, T4, T5, T6>(name);
            keyname.Add<T7>();
            return keyname;
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
            var keyname = Create<T0, T1, T2, T3, T4, T5, T6, T7>(name);
            keyname.Add<T8>();
            return keyname;
        }

        public static Keyname Create<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
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
            var keyname = Create<T0, T1, T2, T3, T4, T5, T6, T7, T8>();
            keyname.Add<T9>();
            return keyname;
        }
        #endregion

        #region operators
        public static implicit operator Keyname(string keyname)
        {
            return Create(keyname);
        }

        public static Keyname operator +(Keyname a, Keyname b)
        {
            var c = a;
            foreach (var keyword in b.keywords)
            {
                c.Add(keyword);
            }

            return c;
        }

        public static Keyname operator +(Keyname a, Keyword b)
        {
            var c = a;
            c.Add(b);
            return c;
        }

        public static Keyname operator -(Keyname a, Keyword b)
        {
            var c = a;
            c.Remove(b);
            return c;
        }

        public static Keyname operator %(Keyname a, Keyword b)
        {
            return a.Filter(b);
        }

        public static Keyname operator %(Keyname a, Keyname b)
        {
            return b.Filter(a);
        }

        public static bool operator ==(Keyname a, Keyname b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Keyname a, Keyname b)
        {
            return !a.Equals(b);
        }
        #endregion
    }
}

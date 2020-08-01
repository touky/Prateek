namespace Prateek.KeynameFramework
{
    using System;
    using System.Diagnostics;
    using Prateek.KeynameFramework.Enums;
    using Prateek.KeynameFramework.Interfaces;
    using UnityEngine.Assertions;

    [DebuggerDisplay("Keyword: {ToString()}:{IntExtensions.ToHex(hash)}")]
    public struct Keyword : IEquatable<Keyword>
    {
        #region Fields
        private Type type;
        private string name;
        private int number;
        private readonly int hash;
        #endregion

        #region Properties
        public KeywordStatus Status
        {
            get { return type != null ? KeywordStatus.Type : KeywordStatus.Name; }
        }

        public Type Type
        {
            get { return type; }
        }

        public string Name
        {
            get { return name; }
        }

        public int Number
        {
            get { return number; }
            set { this = new Keyword(this, value); }
        }
        #endregion

        #region Constructors
        internal Keyword(Keyword other, int number)
        {
            type = other.type;
            name = other.name;
            this.number = number;

            hash = 0;
            hash = BuildHashCode(this);
        }

        public Keyword(Type type) : this(type, 0) { }

        public Keyword(Type type, int number)
        {
            Assert.IsTrue(Match(KeywordRegistry.MasterKeyword, type));

            this.type = type;
            this.number = number;
            name = null;

            hash = 0;
            hash = BuildHashCode(this);
        }

        public Keyword Create<TKeyword>(int number)
            where TKeyword : MasterKeyword
        {
            return new Keyword(typeof(TKeyword), number);
        }

        public Keyword(string name) : this(name, 0) { }

        public Keyword(string name, int number = 0)
        {
            this.name = name;
            this.number = number;
            type = null;

            hash = 0;
            hash = BuildHashCode(this);
        }
        #endregion

        #region Class Methods
        public static implicit operator Keyword(Type type)
        {
            Assert.IsTrue(Match<MasterKeyword>(type));

            return new Keyword(type);
        }

        public static implicit operator Keyword(string name)
        {
            return new Keyword(name);
        }

        private static int BuildHashCode(Keyword other)
        {
            var hash = other.type != null ? other.type.GetHashCode() : other.name.GetHashCode();
            if (other.number != 0)
            {
                hash ^= other.number;
            }

            return hash;
        }

        public override bool Equals(object other)
        {
            if (!(other is Keyword))
            {
                return false;
            }

            return Equals((Keyword) other);
        }

        public static bool operator ==(Keyword a, Keyword b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Keyword a, Keyword b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return hash;
        }

        public override string ToString()
        {
            if (number == 0)
            {
                return type != null ? type.Name : name;
            }

            return type != null ? $"{type.Name}{number}" : $"{name}{number}";
        }

        public string DebugString()
        {
            return Status == KeywordStatus.Type
                ? $"<{ToString()}>"
                : $"\'{ToString()}\'";
        }

        public bool Match(Keyword other)
        {
            if (Status != other.Status)
            {
                return false;
            }

            switch (Status)
            {
                case KeywordStatus.Name:
                {
                    return name == other.Name;
                }
                case KeywordStatus.Type:
                {
                    return Match(type, other.type);
                }
            }

            return false;
        }
        #endregion

        #region IEquatable<Keyword> Members
        public bool Equals(Keyword other)
        {
            var isEqual = false;
            if (type != null && other.type != null)
            {
                isEqual = type == other.type;
            }
            else if (name != null && other.name != null)
            {
                isEqual = name == other.name;
            }

            if (isEqual)
            {
                return number == other.number;
            }

            return false;
        }

        internal static bool Match<TParent>(Type child)
            where TParent : MasterKeyword
        {
            return Match(typeof(TParent), child);
        }

        internal static bool Match(Type parent, Type child)
        {
            return child == parent
                || child.IsSubclassOf(parent)
                || parent.IsAssignableFrom(child);
        }
        #endregion
    }
}

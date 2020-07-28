namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Diagnostics;

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
            get { return type != null ? KeywordStatus.Type : KeywordStatus.String; }
        }

        public Type Type
        {
            get { return type; }
        }

        private string Name
        {
            get { return name; }
        }

        private int Number
        {
            get { return number; }
            set { this = new Keyword(this, value); }
        }
        #endregion

        #region Constructors
        private Keyword(Keyword other, int number)
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
            this.type = type;
            this.number = number;
            name = null;

            hash = 0;
            hash = BuildHashCode(this);
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
        #endregion
    }
}

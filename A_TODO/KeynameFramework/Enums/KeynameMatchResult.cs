namespace Prateek.KeynameFramework.Enums
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Type:{resultType}")]
    public readonly struct KeynameMatchResult : IEquatable<KeynameMatchResult>
    {
        #region Fields
        private readonly KeynameMatchType resultType;
        #endregion

        #region Class Methods
        private KeynameMatchResult(KeynameMatchType resultType)
        {
            this.resultType = resultType;
        }

        public static implicit operator bool(KeynameMatchResult container)
        {
            return container.resultType == KeynameMatchType.Equal;
        }

        public static implicit operator KeynameMatchType(KeynameMatchResult container)
        {
            return container.resultType;
        }

        public static implicit operator KeynameMatchResult(KeynameMatchType resultType)
        {
            return new KeynameMatchResult(resultType);
        }

        public static bool operator >(KeynameMatchResult a, KeynameMatchResult b)
        {
            return a.resultType > b.resultType;
        }

        public static bool operator <(KeynameMatchResult a, KeynameMatchResult b)
        {
            return a.resultType < b.resultType;
        }

        public static bool operator >=(KeynameMatchResult a, KeynameMatchResult b)
        {
            return a.resultType >= b.resultType;
        }

        public static bool operator <=(KeynameMatchResult a, KeynameMatchResult b)
        {
            return a.resultType <= b.resultType;
        }

        public static bool operator >(KeynameMatchResult a, KeynameMatchType b)
        {
            return a.resultType > b;
        }

        public static bool operator <(KeynameMatchResult a, KeynameMatchType b)
        {
            return a.resultType < b;
        }

        public static bool operator >=(KeynameMatchResult a, KeynameMatchType b)
        {
            return a.resultType >= b;
        }

        public static bool operator <=(KeynameMatchResult a, KeynameMatchType b)
        {
            return a.resultType <= b;
        }

        public static bool operator >(KeynameMatchType a, KeynameMatchResult b)
        {
            return a > b.resultType;
        }

        public static bool operator <(KeynameMatchType a, KeynameMatchResult b)
        {
            return a < b.resultType;
        }

        public static bool operator >=(KeynameMatchType a, KeynameMatchResult b)
        {
            return a >= b.resultType;
        }

        public static bool operator <=(KeynameMatchType a, KeynameMatchResult b)
        {
            return a <= b.resultType;
        }

        public static bool operator ==(KeynameMatchResult a, KeynameMatchType b)
        {
            return a.resultType == b;
        }

        public static bool operator !=(KeynameMatchResult a, KeynameMatchType b)
        {
            return a.resultType != b;
        }

        public static bool operator ==(KeynameMatchType a, KeynameMatchResult b)
        {
            return a == b.resultType;
        }

        public static bool operator !=(KeynameMatchType a, KeynameMatchResult b)
        {
            return a != b.resultType;
        }

        public static bool operator ==(KeynameMatchResult a, KeynameMatchResult b)
        {
            return a.resultType == b.resultType;
        }

        public static bool operator !=(KeynameMatchResult a, KeynameMatchResult b)
        {
            return a.resultType != b.resultType;
        }

        public override bool Equals(object other)
        {
            if (!(other is KeynameMatchResult))
            {
                return false;
            }

            return this == (KeynameMatchResult) other;
        }

        public override int GetHashCode()
        {
            return resultType.GetHashCode();
        }
        #endregion

        #region IEquatable<TagMatchResult> Members
        public bool Equals(KeynameMatchResult other)
        {
            return other == this;
        }
        #endregion
    }
}

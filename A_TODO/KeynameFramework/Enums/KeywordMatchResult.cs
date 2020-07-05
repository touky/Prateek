namespace Mayfair.Core.Code.TagSystem
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Type:{resultType}")]
    public struct KeywordMatchResult : IEquatable<KeywordMatchResult>
    {
        #region Fields
        private readonly KeywordMatchResultType resultType;
        #endregion

        #region Class Methods
        private KeywordMatchResult(KeywordMatchResultType resultType)
        {
            this.resultType = resultType;
        }

        public static implicit operator bool(KeywordMatchResult container)
        {
            return container.resultType == KeywordMatchResultType.Equal;
        }

        public static implicit operator KeywordMatchResultType(KeywordMatchResult container)
        {
            return container.resultType;
        }

        public static implicit operator KeywordMatchResult(KeywordMatchResultType resultType)
        {
            return new KeywordMatchResult(resultType);
        }

        public static bool operator >(KeywordMatchResult a, KeywordMatchResult b)
        {
            return a.resultType > b.resultType;
        }

        public static bool operator <(KeywordMatchResult a, KeywordMatchResult b)
        {
            return a.resultType < b.resultType;
        }

        public static bool operator >=(KeywordMatchResult a, KeywordMatchResult b)
        {
            return a.resultType >= b.resultType;
        }

        public static bool operator <=(KeywordMatchResult a, KeywordMatchResult b)
        {
            return a.resultType <= b.resultType;
        }

        public static bool operator >(KeywordMatchResult a, KeywordMatchResultType b)
        {
            return a.resultType > b;
        }

        public static bool operator <(KeywordMatchResult a, KeywordMatchResultType b)
        {
            return a.resultType < b;
        }

        public static bool operator >=(KeywordMatchResult a, KeywordMatchResultType b)
        {
            return a.resultType >= b;
        }

        public static bool operator <=(KeywordMatchResult a, KeywordMatchResultType b)
        {
            return a.resultType <= b;
        }

        public static bool operator >(KeywordMatchResultType a, KeywordMatchResult b)
        {
            return a > b.resultType;
        }

        public static bool operator <(KeywordMatchResultType a, KeywordMatchResult b)
        {
            return a < b.resultType;
        }

        public static bool operator >=(KeywordMatchResultType a, KeywordMatchResult b)
        {
            return a >= b.resultType;
        }

        public static bool operator <=(KeywordMatchResultType a, KeywordMatchResult b)
        {
            return a <= b.resultType;
        }

        public static bool operator ==(KeywordMatchResult a, KeywordMatchResultType b)
        {
            return a.resultType == b;
        }

        public static bool operator !=(KeywordMatchResult a, KeywordMatchResultType b)
        {
            return a.resultType != b;
        }

        public static bool operator ==(KeywordMatchResultType a, KeywordMatchResult b)
        {
            return a == b.resultType;
        }

        public static bool operator !=(KeywordMatchResultType a, KeywordMatchResult b)
        {
            return a != b.resultType;
        }

        public static bool operator ==(KeywordMatchResult a, KeywordMatchResult b)
        {
            return a.resultType == b.resultType;
        }

        public static bool operator !=(KeywordMatchResult a, KeywordMatchResult b)
        {
            return a.resultType != b.resultType;
        }

        public override bool Equals(object other)
        {
            if (!(other is KeywordMatchResult))
            {
                return false;
            }

            return this == (KeywordMatchResult) other;
        }

        public override int GetHashCode()
        {
            return resultType.GetHashCode();
        }
        #endregion

        #region IEquatable<TagMatchResult> Members
        public bool Equals(KeywordMatchResult other)
        {
            return other == this;
        }
        #endregion
    }
}

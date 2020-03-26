namespace Assets.Prateek.ToConvert.TagSystem
{
    using System;
    using System.Diagnostics;

    [DebuggerDisplay("Type:{resultType}")]
    public struct TagMatchResult : IEquatable<TagMatchResult>
    {
        #region Fields
        private readonly TagMatchResultType resultType;
        #endregion

        #region Class Methods
        private TagMatchResult(TagMatchResultType resultType)
        {
            this.resultType = resultType;
        }

        public static implicit operator bool(TagMatchResult container)
        {
            return container.resultType == TagMatchResultType.Equal;
        }

        public static implicit operator TagMatchResultType(TagMatchResult container)
        {
            return container.resultType;
        }

        public static implicit operator TagMatchResult(TagMatchResultType result)
        {
            return new TagMatchResult(result);
        }

        public static bool operator >(TagMatchResult a, TagMatchResult b)
        {
            return a.resultType > b.resultType;
        }

        public static bool operator <(TagMatchResult a, TagMatchResult b)
        {
            return a.resultType < b.resultType;
        }

        public static bool operator >=(TagMatchResult a, TagMatchResult b)
        {
            return a.resultType >= b.resultType;
        }

        public static bool operator <=(TagMatchResult a, TagMatchResult b)
        {
            return a.resultType <= b.resultType;
        }

        public static bool operator >(TagMatchResult a, TagMatchResultType b)
        {
            return a.resultType > b;
        }

        public static bool operator <(TagMatchResult a, TagMatchResultType b)
        {
            return a.resultType < b;
        }

        public static bool operator >=(TagMatchResult a, TagMatchResultType b)
        {
            return a.resultType >= b;
        }

        public static bool operator <=(TagMatchResult a, TagMatchResultType b)
        {
            return a.resultType <= b;
        }

        public static bool operator >(TagMatchResultType a, TagMatchResult b)
        {
            return a > b.resultType;
        }

        public static bool operator <(TagMatchResultType a, TagMatchResult b)
        {
            return a < b.resultType;
        }

        public static bool operator >=(TagMatchResultType a, TagMatchResult b)
        {
            return a >= b.resultType;
        }

        public static bool operator <=(TagMatchResultType a, TagMatchResult b)
        {
            return a <= b.resultType;
        }

        public static bool operator ==(TagMatchResult a, TagMatchResultType b)
        {
            return a.resultType == b;
        }

        public static bool operator !=(TagMatchResult a, TagMatchResultType b)
        {
            return a.resultType != b;
        }

        public static bool operator ==(TagMatchResultType a, TagMatchResult b)
        {
            return a == b.resultType;
        }

        public static bool operator !=(TagMatchResultType a, TagMatchResult b)
        {
            return a != b.resultType;
        }

        public static bool operator ==(TagMatchResult a, TagMatchResult b)
        {
            return a.resultType == b.resultType;
        }

        public static bool operator !=(TagMatchResult a, TagMatchResult b)
        {
            return a.resultType != b.resultType;
        }

        public override bool Equals(object other)
        {
            if (!(other is TagMatchResult))
            {
                return false;
            }

            return this == (TagMatchResult) other;
        }

        public override int GetHashCode()
        {
            return resultType.GetHashCode();
        }
        #endregion

        #region IEquatable<TagMatchResult> Members
        public bool Equals(TagMatchResult other)
        {
            return other == this;
        }
        #endregion
    }
}

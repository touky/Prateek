namespace Mayfair.Core.Editor.ObjectCategorizing
{
    using System;

    [Flags]
    public enum CategoryContentType
    {
        Nothing = 0,

        MATCH = 1 << 0,
        MISS = 1 << 1,
        OVRD = 1 << 2,
        EXTRA = 1 << 3,

        IGNORE = 1 << 4,

        DATA = 1 << 5,
        PROP = 1 << 6,
        LOD = 1 << 7,

        LOD0 = 1 << 8,
        LOD1 = 1 << 9,
        LOD2 = 1 << 10,
        LOD3 = 1 << 11,
        LODN = 1 << 12,
        
        LODALL = LOD0 | LOD1 | LOD2 | LOD3,

    }

    public static class CategoryContentTypeExtensions
    {
        #region Class Methods
        public static bool HasEither(this CategoryContentType status, CategoryContentType other) { return (status & other) != CategoryContentType.Nothing; }
        public static bool HasBoth(this CategoryContentType status, CategoryContentType other) { return (status & other) == other; }
        public static void Add(ref this CategoryContentType status, CategoryContentType other) { status |= other; }
        public static void Remove(ref this CategoryContentType status, CategoryContentType other) { status &= ~other; }
        #endregion
    }
}

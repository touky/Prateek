namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums
{
    using System;

    [Flags]
    public enum SearchValidation
    {
        Nothing = 0,

        ShaderRef = 1 << 0,
        ShaderName = 1 << 1,
        MaterialName = 1 << 2
    }

    public static class SearchValidationExtensions
    {
        #region Class Methods
        public static bool Has(this SearchValidation status, SearchValidation other)
        {
            return (status & other) != SearchValidation.Nothing;
        }

        public static bool Has(this EditorAction status, EditorAction other)
        {
            return (status & other) != EditorAction.Nothing;
        }

        public static bool Has(this SortBy status, SortBy other)
        {
            return (status & other) != SortBy.Nothing;
        }
        #endregion
    }
}

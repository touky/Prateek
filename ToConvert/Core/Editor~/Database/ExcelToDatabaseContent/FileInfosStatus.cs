namespace Mayfair.Core.Editor.Database.ExcelToDatabaseContent
{
    using System;

    [Flags]
    internal enum FileInfosStatus
    {
        Nothing = 0,

        WasWritten = 1 << 0,
        ForceRebuild = 1 << 1,
        ShouldIgnore = 1 << 2,

        ALL = ~0
    }

    internal static class FileInfosStatusExtensions
    {
        // @formatter:off
        public static bool HasEither(this FileInfosStatus status, FileInfosStatus other) { return (status & other) != FileInfosStatus.Nothing; }
        public static bool HasBoth(this FileInfosStatus status, FileInfosStatus other) { return (status & other) == other; }
        public static void Add(ref this FileInfosStatus status, FileInfosStatus other) { status |= other; }
        public static void Remove(ref this FileInfosStatus status, FileInfosStatus other) { status &= ~other; }
        // @formatter:on
    }
}

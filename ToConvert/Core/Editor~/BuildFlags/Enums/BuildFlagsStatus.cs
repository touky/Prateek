namespace Mayfair.Core.Editor.BuildFlags.Enums
{
    using System;

    [Flags]
    public enum BuildFlagsStatus
    {
        Nothing = 0,

        NeedSymbolUpdate = 1 << 0,
        NeedRepaint = 1 << 1,
        ApplySymbols = 1 << 2,
        PendingSave = 1 << 3,
        NeedSettingsSave = 1 << 4,

        MAX
    }

    internal static class BuildFlagsStatusExtensions
    {
        // @formatter:off
        public static bool HasEither(this BuildFlagsStatus status, BuildFlagsStatus other) { return (status & other) != BuildFlagsStatus.Nothing; }
        public static bool HasBoth(this BuildFlagsStatus status, BuildFlagsStatus other) { return (status & other) == other; }
        public static void Add(ref this BuildFlagsStatus status, BuildFlagsStatus other) { status |= other; }
        public static void Remove(ref this BuildFlagsStatus status, BuildFlagsStatus other) { status &= ~other; }
        // @formatter:on
    }
}

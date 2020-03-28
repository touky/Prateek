namespace Mayfair.Core.Editor.EditorWindows {
    using System;

    [Flags]
    public enum FieldSlotTag
    {
        Nothing = 0,

        Enabled = 1 << 0,
        DataFile = 1 << 1,
        NeedExistenceCheck = 1 << 2,
        AddSpaceBefore = 1 << 3,
        RelativeToAssetPath = 1 << 4,
        IsType = 1 << 5,
        IsDirectory = 1 << 6,
        IsFile = 1 << 7,
        Toggleable = 1 << 8,
        ToggleDefaultOn = 1 << 9,
        GroupStart = 1 << 10,
        Group = 1 << 11,
        NeedCheckout = 1 << 12,
        RelativeToOtherPath = 1 << 13,

        ALL = ~0
    }

    public static class FieldSlotTagExtensions
    {
        // @formatter:off
        public static bool HasEither(this FieldSlotTag status, FieldSlotTag other) { return (status & other) != FieldSlotTag.Nothing; }
        public static bool HasBoth(this FieldSlotTag status, FieldSlotTag other) { return (status & other) == other; }
        public static void Add(ref this FieldSlotTag status, FieldSlotTag other) { status |= other; }
        public static void Remove(ref this FieldSlotTag status, FieldSlotTag other) { status &= ~other; }
        // @formatter:on
    }
}
namespace Mayfair.Core.Code.Utils
{
    using System;

    /// <summary>
    ///     EnumAsFlags is a mockup enum to help creating more practical Enum as flags
    ///     Copy this block in you C# code and replace all EnumAsFlags occurences
    /// </summary>
    [Flags]
    internal enum EnumAsFlags
    {
        Nothing = 0,

        MyValue0 = 1 << 0,
        MyValue1 = 1 << 1,

        ALL = ~0
    }

    internal static class EnumAsFlagsExtensions
    {
        // @formatter:off
        public static bool HasEither(this EnumAsFlags status, EnumAsFlags other) { return (status & other) != EnumAsFlags.Nothing; }
        public static bool HasBoth(this EnumAsFlags status, EnumAsFlags other) { return (status & other) == other; }
        public static void Add(ref this EnumAsFlags status, EnumAsFlags other) { status |= other; }
        public static void Remove(ref this EnumAsFlags status, EnumAsFlags other) { status &= ~other; }
        // @formatter:on
    }
}

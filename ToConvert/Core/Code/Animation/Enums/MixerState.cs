namespace Mayfair.Core.Code.Animation.Enums {
    using System;

    [Flags]
    internal enum MixerState
    {
        Nothing = 0,

        HasFullBody = 1 << 0,
        HasAdditive = 1 << 1,
        HasPoseHandler = 1 << 2,

        ALL = ~0
    }

    internal static class MixerStateExtensions
    {
        // @formatter:off
        public static bool HasEither(this MixerState status, MixerState other) { return (status & other) != MixerState.Nothing; }
        public static bool HasBoth(this MixerState status, MixerState other) { return (status & other) == other; }
        public static void Add(ref this MixerState status, MixerState other) { status |= other; }
        public static void Remove(ref this MixerState status, MixerState other) { status &= ~other; }
        // @formatter:on
    }
}
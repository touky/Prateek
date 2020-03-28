namespace Mayfair.Core.Editor.VisualAsset {
    using System;

    [Flags]
    public enum VisualAssetMissingContent
    {
        Nothing = 0,

        basePlate = 1 << 0,
        lodReferences = 1 << 1,
        animationTransforms = 1 << 2,
        Animator = 1 << 3,
        VisualGraph = 1 << 4,
        Colliders = 1 << 5,
    }

    public static class VisualAssetMissingContentExtensions
    {
        // @formatter:off
        public static bool HasEither(this VisualAssetMissingContent status, VisualAssetMissingContent other) { return (status & other) != VisualAssetMissingContent.Nothing; }
        public static bool HasBoth(this VisualAssetMissingContent status, VisualAssetMissingContent other) { return (status & other) == other; }
        public static void Add(ref this VisualAssetMissingContent status, VisualAssetMissingContent other) { status |= other; }
        public static void Remove(ref this VisualAssetMissingContent status, VisualAssetMissingContent other) { status &= ~other; }
        // @formatter:on
    }
}
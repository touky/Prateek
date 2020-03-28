namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums
{
    using System;

    [Flags]
    public enum SortBy
    {
        Nothing = 0,

        Inverted = 1 << 0,

        Shader = 1 << 1,
        Material = 1 << 2,
        Location = 1 << 3
    }
}

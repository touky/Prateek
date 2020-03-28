namespace Mayfair.Core.Editor.BuildTools.SceneManagement
{
    using System;

    [Flags]
    public enum BuildType
    {
        None = 0,
        Runtime = 1 << 0,
        UnitTest = 1 << 1,
        Gym = 1 << 2
    }
}
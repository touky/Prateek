namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums
{
    using System;

    [Flags]
    public enum EditorAction
    {
        Nothing = 0,

        SetDirty = 1 << 0,
        Checkout = 1 << 1,
        RecordUndo = 1 << 2,
        Select = 1 << 3,
        DoAction = 1 << 4
    }
}

namespace Prateek.DaemonCore.Code
{
    using System;

    [Flags]
    public enum TickType
    {
        BeginUpdate = 1 << 0,
        BeginUpdateLate = 1 << 1,
        BeginUpdateFixed = 1 << 2,
        EndUpdate = 1 << 3,
        EndUpdateLate = 1 << 4,
        EndUpdateFixed = 1 << 5,
        OnApplicationFocus = 1 << 6,
        OnApplicationPause = 1 << 7,
        OnApplicationQuit = 1 << 8,
        OnGUI = 1 << 9,

        ALL = ~0,

        OFFSET_BEGIN = 0,
        OFFSET_END = 3
    }
}

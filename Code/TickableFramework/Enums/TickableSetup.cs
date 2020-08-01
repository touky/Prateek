namespace Prateek.TickableFramework.Code.Enums
{
    using System;

    [Flags]
    public enum TickableSetup
    {
        UpdateBegin = 1 << 0,
        UpdateBeginLate = 1 << 1,
        UpdateBeginFixed = 1 << 2,
        UpdateEnd = 1 << 3,
        UpdateEndLate = 1 << 4,
        UpdateEndFixed = 1 << 5,
        OnApplicationFocus = 1 << 6,
        OnApplicationPause = 1 << 7,
        OnApplicationQuit = 1 << 8,
        OnGUI = 1 << 9,

        MAX = OnGUI << 1,

        Everything = ~0,
        Nothing = 0,

        OFFSET_BEGIN = 0,
        OFFSET_END = 3
    }
}

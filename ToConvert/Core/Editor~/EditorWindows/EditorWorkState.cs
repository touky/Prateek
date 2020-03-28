namespace Mayfair.Core.Editor.EditorWindows
{
    /// <summary>
    /// Use these state to implement your EditorWindow behaviours.
    /// Use the "WorkTaskCustom + N" slots for custom operations
    /// WorkTaskCustom + [1 - 99] are open for these cases
    /// </summary>
    public enum EditorWorkState
    {
        Init,
        CheckForCompiling,
        WaitForCompiling,
        AutoExecute,
        WaitForAutoExecute,
        ShouldStartWork,
        WorkTaskProcessing,
        WaitForWorkTaskProcessing,
        WorkTaskCustom,
        //Leaving 100 slots open
        AutoStop = WorkTaskCustom + 100,
        WaitForAutoStop,
        Idle
    }
}

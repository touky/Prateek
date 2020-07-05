namespace Mayfair.Core.Code.LoadingProcess.Enums
{
    public enum LoadingTrackingStatus
    {
        Nothing,

        StartedLoading,
        LoadingPrerequisite,
        HasLoadedPrerequisite,
        LoadingMain,
        HasLoadedMain,
        PostProcessing,

        Finished
    }
}

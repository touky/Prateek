namespace Prateek.Editor.Settings
{
    using UnityEditor;

    internal static class PrateekStopPlayingOnCompile
    {
        #region Class Methods
        [InitializeOnLoadMethod]
        private static void InitOnLoad()
        {
            if (EditorApplication.isPlaying)
            {
                if (PrateekSettings.StopPlayModeOnScriptCompile)
                {
                    EditorApplication.isPlaying = false;
                }
            }
        }
        #endregion
    }
}

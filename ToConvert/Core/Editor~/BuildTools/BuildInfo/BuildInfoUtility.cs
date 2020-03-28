namespace Mayfair.Core.Editor.BuildTools.BuildInfo
{
    using Mayfair.Core.Code.BuildTools;
    using Utils;
    using UnityEditor;
    using UnityEngine;
    using Versioner;

    public static class BuildInfoUtility
    {
        private const string AUTOBUILDER_FILENAME = "AutoBuilder";
        private const string AUTOBUILDER_RELATIVE_RESOURCE_PATH = "/Resources";

        private static BuildInfo buildInfo;

        /// <remarks>
        ///     Unfortunately Unity does not natively allow us to access some useful build information at runtime.
        /// </remarks>
        /// <summary>
        ///     Dumps build info into a scriptable object so that we can use it at runtime.
        /// </summary>
        public static void UpdateBuildInfo(string revision)
        {
            // Must check to see if the object is already in memory because ScriptableObjects can persist
            // only reload if needed
            if (buildInfo == null)
            {
                buildInfo = Resources.Load<BuildInfo>(BuildInfo.BUILDINFO_FILENAME);

                // Create object in proper directory if needed
                if (buildInfo == null)
                {
                    string[] autobuilderFile = AssetDatabase.FindAssets(string.Format("{0} t:folder", AUTOBUILDER_FILENAME));
                    string autoBuilderPath = AssetDatabase.GUIDToAssetPath(autobuilderFile[0]);
                    string resourcePath = autoBuilderPath + AUTOBUILDER_RELATIVE_RESOURCE_PATH;
                    AssetMenuHelper.CreateAsset<BuildInfo>(resourcePath, BuildInfo.BUILDINFO_FILENAME);
                    buildInfo = Resources.Load<BuildInfo>(BuildInfo.BUILDINFO_FILENAME);
                }
            }

            UpdateVersionInfo(ParseRevisionNumber(revision));

            buildInfo.iOSBuildNumber = PlayerSettings.iOS.buildNumber;
            buildInfo.AndroidBundleVersion = PlayerSettings.Android.bundleVersionCode;
        }

        private static void LogOutput(string msg)
        {
            Debug.LogFormat("  [BuildInfoUtility] - {0}", msg);
        }

        private static int ParseRevisionNumber(string revision)
        {
            const int FALLBACK_REVISION = 1;
            int revisionNumber = 0;

            if (!string.IsNullOrEmpty(revision))
            {
                if (int.TryParse(revision, out revisionNumber))
                {
                    LogOutput($"Parsed revision number {revision}, updating build version.");
                }
                else
                {
                    LogOutput($"Error: failed to parse revision number {revision}. Setting revision to {FALLBACK_REVISION}");
                    revisionNumber = FALLBACK_REVISION;
                }
            }
            else
            {
                LogOutput($"Error: No revision value provided. Setting revision to {FALLBACK_REVISION}");
                revisionNumber = FALLBACK_REVISION;
            }

            return revisionNumber;
        }

        /// <summary>
        ///     Sets the revision number in the Version info for the app.
        ///     Body is declared with the Versioner module.
        /// </summary>
        private static void UpdateVersionInfo(int revision)
        {
            Versioner.SetBuildNumber(revision);
        }
    }
}
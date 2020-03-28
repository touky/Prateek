namespace Mayfair.Core.Editor.BuildTools.Versioner
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Manages the App Version information. Can be called from another system or 
    /// invoked through the -executeMethod argument on the command line
    /// This script depends on the iOS version information to track and store the version and build information.
    /// </summary>
    [InitializeOnLoad]
    public static class Versioner
    {
        private const int MAJ_WARNING_THRESHOLD = 18;
        private const int MAJ_ERROR_THRESHOLD = 21;
        private const int MIN_WARNING_THRESHOLD = 95;
        private const int PATCH_WARNING_THRESHOLD = 95;

        public static void IncrementMajor()
        {
            IncrementVersion(true, false, false, false, true);
        }

        public static void IncrementMinor()
        {
            IncrementVersion(false, true, false, false, true);
        }

        public static void IncrementPatch()
        {
            IncrementVersion(false, false, true, false, true);
        }

        public static void IncrementBuild()
        {
            IncrementVersion(false, false, false, true, false);
        }

        /// <summary>
        /// Sets the build portion of the version number to the provided value
        /// {Major}.{Minor}.{Patch}.{Build}
        /// </summary>
        public static void SetBuildNumber(int buildNumber)
        {
            string[] lines = SplitBundleVersion();

            int majorVersion = 0;
            int minorVersion = 0;
            int patchVersion = 0;

            if (lines.Length >= 1)
            {
                majorVersion = int.Parse(lines[0]);
            }
            if (lines.Length >= 2)
            {
                minorVersion = int.Parse(lines[1]);
            }
            if (lines.Length >= 3)
            {
                patchVersion = int.Parse(lines[2]);
            }

            SetVersion(majorVersion, minorVersion, patchVersion, buildNumber);
        }

        public static string GetVersionWarning()
        {
            string warning = string.Empty;
            string[] splitVersion = SplitBundleVersion();

            if (splitVersion.Length >= 1 && int.Parse(splitVersion[0]) > MAJ_ERROR_THRESHOLD)
            {
                throw new System.ArgumentOutOfRangeException("The Major Version number for the app cannot exceed 21. Congratz on hit this major milestone, maybe you should go have a drink!");
            }
            else if(splitVersion.Length >= 2 && int.Parse(splitVersion[0]) >= MAJ_WARNING_THRESHOLD)
            {
                warning = "You are approaching the Major Version cap of 21";
            }
            else if (splitVersion.Length >= 3 && int.Parse(splitVersion[1]) >= MIN_WARNING_THRESHOLD)
            {
                warning = "You are approaching the Minor Version cap of 99. Consider rolling over soon.";
            }
            else if (splitVersion.Length >= 4 && int.Parse(splitVersion[2]) >= PATCH_WARNING_THRESHOLD)
            {
                warning = "You are approaching the Patch Version cap of 99. Consider rolling over soon.";
            }

            return warning;
        }

        private static void IncrementVersion(bool majorIncr, bool minorIncr, bool patchIncr, bool buildIncr, bool resetBuild = false)
        {
            string[] lines = SplitBundleVersion();

            int majorVersion = 0;
            int minorVersion = 0;
            int patchVersion = 0;
            int build = 0;

            if (lines.Length >= 1)
            {
                majorVersion = int.Parse(lines[0]);
            }
            if (lines.Length >= 2)
            {
                minorVersion = int.Parse(lines[1]);
            }
            if (lines.Length >= 3)
            {
                patchVersion = int.Parse(lines[2]);
            }

            if (majorIncr)
            {
                majorVersion++;
                minorVersion = 0;
                patchVersion = 0;
            }
            if (minorIncr)
            {
                minorVersion++;
                patchVersion = 0;
            }

            if (patchIncr)
            {
                patchVersion++;
            }

            if (resetBuild)
            {
                build = 0;
            }
            else
            {
                if (lines.Length >= 4)
                {
                    build = int.Parse(lines[3]);
                }

                if (buildIncr)
                {
                    build++;
                }
            }

            SetVersion(majorVersion, minorVersion, patchVersion, build);
        }

        // Keep for testing with new unity version
        private static string[] SplitIosVersion()
        {
            return PlayerSettings.iOS.buildNumber.Split('.');
        }

        private static string[] SplitBundleVersion()
        {
            return PlayerSettings.bundleVersion.Split('.');
        }

        /// <summary>
        /// Constructs the various version formats and sets them in the appropriate settings.
        /// </summary>
        /// <remarks>Build will be clamped between 0 and 9999 to avoid overflowing the integer Android Bundle Version Code</remarks>
        private static void SetVersion(int majorVersion, int minorVersion, int patchVersion, int build)
        {
            // Will throw exception if the version blows the integer limit
            string warning = GetVersionWarning();
            if (warning != string.Empty)
            {
                Debug.LogWarning(warning);
            }

            // This value is shared between iOS and Android
            PlayerSettings.iOS.buildNumber = string.Format("{0}.{1}.{2}.{3}", majorVersion, minorVersion, patchVersion, build);
            PlayerSettings.bundleVersion = string.Format("{0}.{1}.{2}", majorVersion, minorVersion, patchVersion);

            PlayerSettings.Android.bundleVersionCode = build;

            LogOutput(string.Format("Update project version to {0}.{1}.{2}.{3}", majorVersion, minorVersion, patchVersion, build));
        }

        private static void LogOutput(string msg)
        {
            Debug.LogFormat("  [Versioner] - {0}", msg);
        }
    }
}
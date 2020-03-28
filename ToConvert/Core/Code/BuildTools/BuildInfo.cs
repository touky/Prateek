namespace Mayfair.Core.Code.BuildTools
{
    using UnityEditor;
    using UnityEngine;
#if UNITY_EDITOR

#endif

    /// <summary>
    ///     The majority of version info in Unity is locked up in the Editor.
    ///     This object stores a copy of the data for runtime use.
    /// </summary>
    public class BuildInfo : ScriptableObject
    {
        public const string BUILDINFO_FILENAME = "BuildInfo";
        private const int VERSION_LENGTH = 2;
        private const int BUILD_LENGTH = 4;
        private const int MAJOR_INDEX = 0;
        private const int MINOR_INDEX = MAJOR_INDEX + VERSION_LENGTH;
        private const int PATCH_INDEX = MINOR_INDEX + VERSION_LENGTH;
        private const int BUILD_INDEX = PATCH_INDEX + VERSION_LENGTH;

        private static BuildInfo loadedData;

#pragma warning disable 0414
        [SerializeField]
        private string iosBuildNumber;

        [SerializeField]
        private int androidBundleVersion;
#pragma warning restore 0414

        /// <summary>
        ///     Returns Application.version. This version is shared across platforms (its the CFBundleShortVersionString on iOS)
        /// </summary>
        public string AppVersion
        {
            get => Application.version;
        }

        /// <summary>
        ///     Returns Application.unityVersion
        /// </summary>
        public string UnityVersion
        {
            get => Application.unityVersion;
        }

        public int AndroidBundleVersion
        {
            get
            {
#if UNITY_EDITOR
                return PlayerSettings.Android.bundleVersionCode;
#else
                return androidBundleVersion;
#endif
            }
            set { this.androidBundleVersion = value; }
        }

        public string iOSBuildNumber
        {
            get
            {
#if UNITY_EDITOR
                return PlayerSettings.iOS.buildNumber;
#else
                return iosBuildNumber;
#endif
            }
            set { this.iosBuildNumber = value; }
        }

        /// <summary>
        ///     Returns the full version number with build number as a dot separated value: X.X.X.XXX
        /// </summary>
        /// <remarks> We always use the iOS version as it is in the format we desire</remarks>
        public string DotSeparatedFullVersion
        {
            get => iOSBuildNumber;
        }

        public static BuildInfo LoadedData
        {
            get
            {
                if (loadedData == null)
                {
                    loadedData = Resources.Load<BuildInfo>(BUILDINFO_FILENAME);
                }

                return loadedData;
            }
        }
    }
}
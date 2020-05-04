namespace Mayfair.Core.Editor.BuildTools.BuildInfo
{
    using Mayfair.Core.Code.BuildTools;
    using Utils;
    using UnityEditor;

    public static class BuildInfoMenuItem
    {
        [MenuItem("Tools/AutoBuilder/Scriptable Objects/Create VersionInfo Object")]
        public static void VersionInfo()
        {
            AssetMenuHelper.CreateAsset<BuildInfo>();
        }
    }
}

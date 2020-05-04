namespace Mayfair.Core.Code.Utils
{
    public static class ConstsFolders
    {
        #region Static and Constants
        public const string TEMP = "~";
        public const string ASSET = "Assets";
        public const string SCRIPTS = "Scripts";
        public const string PLUGINS = "Plugins";
        public const string PREFABS = "Prefabs";
        public const string SCENES = "Scenes";
        public const string EXPORT = "Export";
        public const string EDITOR = "Editor";
        public const string UNITTESTS = "UnitTests";
        public const string GENERATED = "Generated";
        public const string GENERATED_STATICS = "GeneratedStatics";
        public const string LIBRARY = "Library";
        public const string RESOURCES = "Resources";

        public const string ART_ROOT = "Art";
        public const string ART_EXPORT_ROOT = "GameReady";

        /// <summary>
        ///     This is ripped off "com.unity.scriptablebuildpipeline@1.5.4\Editor\Utilities\CommonStrings.cs"
        ///     This is also the reason why it's not using the coding standard naming rules
        /// </summary>
        public const string UnityEditorResourcePath = "library/unity editor resources";
        public const string UnityDefaultResourcePath = "library/unity default resources";
        public const string UnityBuiltInExtraPath = "resources/unity_builtin_extra";
        #endregion
    }
}

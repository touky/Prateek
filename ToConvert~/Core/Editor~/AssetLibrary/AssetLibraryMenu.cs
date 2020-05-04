namespace Mayfair.Core.Editor.AssetLibrary
{
    using Mayfair.Core.Code.Animation;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.VisualAsset;
    using Mayfair.Core.Editor.Animation;
    using Mayfair.Core.Editor.CustomImportSettings;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    public static class AssetLibraryMenu
    {
        public const string RESOURCE_PATH = "Editor/";
        private const string CUSTOM_IMPORT_SETTINGS = "CustomImportSettings";
        private const string PATH_NAME = "AssetLibrary";
        public const string LIBRARY_PATH = PATH_NAME + "/";
        public const string LOCATION_PATH = RESOURCE_PATH + LIBRARY_PATH;

        [MenuItem(AssetMenuHelper.ASSET_MENU + "Library/Fbx" + CUSTOM_IMPORT_SETTINGS)]
        public static ScriptableObject CreateFbxCustomImportSettings()
        {
            return AssetMenuHelper.CreateAsset<FbxCustomImportSettings>(LOCATION_PATH);
        }

        [MenuItem(AssetMenuHelper.ASSET_MENU + "Library/Texture" + CUSTOM_IMPORT_SETTINGS)]
        public static ScriptableObject CreateTextureCustomImportSettings()
        {
            return AssetMenuHelper.CreateAsset<TextureCustomImportSettings>(LOCATION_PATH);
        }

        [MenuItem(AssetMenuHelper.ASSET_MENU + "Library/Animation" + CUSTOM_IMPORT_SETTINGS)]
        public static ScriptableObject CreateAnimationCustomImportSettings()
        {
            return AssetMenuHelper.CreateAsset<AnimationCustomImportSettings>(LOCATION_PATH);
        }

        [MenuItem(AssetMenuHelper.ASSET_MENU + "Library/" + PATH_NAME)]
        public static ScriptableObject CreateAssetLibrary()
        {
            return AssetMenuHelper.CreateAsset<AssetLibrary>(LOCATION_PATH);
        }

        [MenuItem(AssetMenuHelper.ASSET_MENU + "Library/" + PATH_NAME + "Storage")]
        public static ScriptableObject CreateAssetLibraryStorage()
        {
            return AssetMenuHelper.CreateAsset<AssetLibraryStorage>();
        }

        [MenuItem(AssetMenuHelper.ASSET_MENU + "AnimationLibrary")]
        public static ScriptableObject CreateAnimationLibraryStorage()
        {
            return AssetMenuHelper.CreateAsset<AnimationLibrary>();
        }
    }
}
namespace Mayfair.Core.Editor.Utils
{
    using System;
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Build;
    using UnityEditor.AddressableAssets.Settings;

    public static class UnityAddressablesHelper
    {
        #region Class Methods
        /// <summary>
        ///     This validates if the path of this addressable entry is a valid one -i.e. this can have an address-
        /// </summary>
        /// <remarks>
        ///     This is ripped off "com.unity.addressables@1.4.0\Editor\Settings\AddressableAssetUtility.cs"
        /// </remarks>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsPathValidForEntry(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return false;
            }

            path = path.ToLower();
            if (path == ConstsFolders.UnityEditorResourcePath ||
                path == ConstsFolders.UnityDefaultResourcePath ||
                path == ConstsFolders.UnityBuiltInExtraPath)
            {
                return false;
            }

            string ext = Path.GetExtension(path);
            if (ext == ".cs" || ext == ".js" || ext == ".boo" || ext == ".exe" || ext == ".dll")
            {
                return false;
            }

            Type t = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (t != null && BuildUtility.IsEditorAssembly(t.Assembly))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     This returns the Path & GUID for a specific target
        /// </summary>
        /// <remarks>
        ///     This is ripped off "com.unity.addressables@1.4.0\Editor\Settings\AddressableAssetUtility.cs"
        /// </remarks>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool GetPathAndGUIDFromTarget(UnityEngine.Object target, out string path, ref string guid, out Type mainAssetType)
        {
            mainAssetType = null;
            path = AssetDatabase.GetAssetOrScenePath(target);
            if (!IsPathValidForEntry(path))
            {
                return false;
            }

            guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid))
            {
                return false;
            }

            mainAssetType = AssetDatabase.GetMainAssetTypeAtPath(path);
            if (mainAssetType != target.GetType() && !typeof(AssetImporter).IsAssignableFrom(target.GetType()))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     This finds and entry for the given target in the addressable settings
        /// </summary>
        /// <remarks>
        ///     This is Ripped off "com.unity.addressables@1.4.0\Editor\GUI\AssetInspectorGUI.cs"
        /// </remarks>
        /// <param name="target"></param>
        /// <returns></returns>
        public static AddressableAssetEntry FindEntry(AddressableAssetSettings addressableSettings, UnityEngine.Object target)
        {
            AddressableAssetEntry entry = null;
            string guid = string.Empty;
            string path;
            Type mainAssetType;
            if (GetPathAndGUIDFromTarget(target, out path, ref guid, out mainAssetType) &&
                path.ToLower().Contains("assets") &&
                mainAssetType != null)
            {
                // Is asset
                if (!BuildUtility.IsEditorAssembly(mainAssetType.Assembly))
                {
                    if (addressableSettings != null)
                    {
                        entry = addressableSettings.FindAssetEntry(guid);
                    }
                }
            }

            return entry;
        }

        public static AddressableAssetEntry FindEntry(UnityEngine.Object target)
        {
            AddressableAssetSettings addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            if (addressableSettings == null)
            {
                return null;
            }

            return FindEntry(addressableSettings, target);
        }
        #endregion
    }
}

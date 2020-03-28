namespace Mayfair.Core.Editor.Addressables
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text.RegularExpressions;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEditor.AddressableAssets.Settings.GroupSchemas;
    using UnityEngine;

    public static class AddressableHelper
    {
        #region Static and Constants
        public const string SEPARATOR = "/";
        public const string DEFAULT = "Default";
        public const string UNKNOWN_RESOURCE_GROUP = "UnknownResources";
        public const string DEFAULT_RESOURCE_GROUP = "DefaultResources";
        #endregion

        #region Class Methods
        public static string GetGroupFromName(string assetName, string groupKeyword)
        {
            string groupName = string.Empty;
            Match match = RegexHelper.PrefixDetect.Match(assetName);
            if (!match.Success && assetName.StartsWith(DEFAULT))
            {
                groupName = DEFAULT_RESOURCE_GROUP;
            }
            else if (!match.Success)
            {
                groupName = UNKNOWN_RESOURCE_GROUP;
            }
            else
            {
                groupName = $"{match.Value}{groupKeyword}";
            }

            return groupName;
        }

        public static void SetAddress(string assetPath)
        {
            SetAddress(assetPath, string.Empty, string.Empty);
        }

        public static void SetAddress(string assetPath, string address)
        {
            SetAddress(assetPath, address, string.Empty);
        }

        public static void SetAddress(Object target, string address, string groupName)
        {
            SetAddress(AssetDatabase.GetAssetPath(target), address, groupName);
        }

        public static void SetAddressWithKeyword(UnityEngine.Object asset, string keyword, string assetName)
        {
            string simpleName = Path.GetFileNameWithoutExtension(assetName);
            string address = $"{keyword}{simpleName}";
            string group = AddressableHelper.GetGroupFromName(simpleName, keyword.Remove(keyword.Length - 1));
            AddressableHelper.SetAddress(AssetDatabase.GetAssetPath(asset), address, group);

        }

        public static void SetAddress(string assetPath, string address, string groupName)
        {
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                return;
            }

            AddressableAssetEntry entry = CreateOrUpdateAddressableAssetEntry(settings, groupName, assetPath, address);
            if (entry != null)
            {
                List<AddressableAssetEntry> entriesAdded = new List<AddressableAssetEntry> {entry};
                settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true);
                AssetDatabase.SaveAssets();
            }
        }

        private static bool TryGetGroup(AddressableAssetSettings settings, string groupName, out AddressableAssetGroup group)
        {
            if (string.IsNullOrWhiteSpace(groupName))
            {
                group = settings.DefaultGroup;
                return true;
            }

            return (group = settings.groups.Find(g => string.Equals(g.Name, groupName.Trim()))) == null ? false : true;
        }

        private static AddressableAssetGroup CreateAssetGroup<SchemaType>(AddressableAssetSettings settings, string groupName)
        {
            return settings.CreateGroup(groupName, false, false, false, new List<AddressableAssetGroupSchema> {settings.DefaultGroup.Schemas[0]}, typeof(SchemaType));
        }

        private static AddressableAssetEntry CreateOrUpdateAddressableAssetEntry(AddressableAssetSettings settings, string groupName, string assetPath, string assetAddress)
        {
            // Set group
            AddressableAssetGroup group;
            if (groupName != string.Empty)
            {
                if (!TryGetGroup(settings, groupName, out group))
                {
                    group = CreateAssetGroup<BundledAssetGroupSchema>(settings, groupName);
                }
            }
            else
            {
                group = settings.DefaultGroup;
            }

            string guid = AssetDatabase.AssetPathToGUID(assetPath);
            AddressableAssetEntry entry = settings.CreateOrMoveEntry(guid, group);

            // Apply address replacement if address is empty or path.
            if (assetAddress != string.Empty)
            {
                entry.address = assetAddress;
            }

            //TODO: benjaminh: (02/03/2020) Right now groups are supported but not labels.
            //TODO: Labels don't seem like a useful thing right now, but maybe we should think about it in the future.

            return entry;
        }
        #endregion
    }
}

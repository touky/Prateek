namespace Mayfair.Core.Editor.AssetLibrary
{
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.Utils.Helpers.Regexp;
    using Mayfair.Core.Editor.Utils;
    using Mayfair.Core.Editor.EditorService;
    using UnityEditor;
    using UnityEngine;

    public class AssetLibraryStorage : EditorServiceProvider
    {
        #region Settings
        [SerializeField]
        [HideInInspector]
        private List<AssetLibraryItem> items = new List<AssetLibraryItem>();
        #endregion

        #region Properties
        protected override double RefreshTimer
        {
            get { return 5; }
        }
        #endregion

        #region Class Methods
        public AssetLibraryItem GetItem(string name)
        {
            TryReloadContent();

            List<string> matches = new List<string>();
            if (!RegexHelper.TryFetchingMatches(name, RegexHelper.AssetTag, matches))
            {
                return null;
            }

            AssetLibraryItem item = FindMatchingLibraryItem(matches);
            if (item == null || item.Replacement == null)
            {
                return null;
            }

            return item;
        }

        private AssetLibraryItem FindMatchingLibraryItem(List<string> matches)
        {
            foreach (AssetLibraryItem item in items)
            {
                if (!item.Enabled)
                {
                    continue;
                }

                List<string> tags = item.Tags;
                if (matches.Count != tags.Count)
                {
                    continue;
                }

                bool doesMatch = true;
                for (int t = 0; t < tags.Count; t++)
                {
                    if (matches[t].ToLower() != tags[t].ToLower())
                    {
                        doesMatch = false;
                        break;
                    }
                }

                if (!doesMatch)
                {
                    continue;
                }

                return item;
            }

            return null;
        }

        public void Cleanup(AssetLibraryItem item = null)
        {
            items.RemoveAll(x =>
            {
                return item == null
                    ? x.Replacement == null
                    : x == item;
            });
        }

        public override void LoadContent()
        {
            base.LoadContent();

            string thisPath = AssetDatabase.GetAssetPath(this);
            string thisDir = PathHelper.Simplify(Path.GetDirectoryName(thisPath));
            string[] allStorages = AssetDatabase.FindAssets($"t:{typeof(AssetLibraryStorage).Name}", new[] {thisDir});
            HashSet<string> storagePaths = new HashSet<string>();
            foreach (string storage in allStorages)
            {
                string path = AssetDatabase.GUIDToAssetPath(storage);
                if (path == thisPath)
                {
                    continue;
                }

                storagePaths.Add(PathHelper.Simplify(Path.GetDirectoryName(path)));
            }

            string[] allPaths = AssetDatabase.GetAllAssetPaths();
            HashSet<string> subAssetPaths = new HashSet<string>();
            //Grab all storage in subfolders
            FindAllContentInSubFolders(thisDir, allPaths, storagePaths, subAssetPaths);

            HashSet<GameObject> prefabs = new HashSet<GameObject>();
            //Grab all asset in our hierarchy, ignore folders that already contains a storage
            LoadFilteredContent(subAssetPaths, prefabs);

            SortAndMarkItems(prefabs);
        }

        private void FindAllContentInSubFolders(string sourceDir, string[] allPaths, HashSet<string> storagePaths, HashSet<string> subAssetPaths)
        {
            foreach (string assetPath in allPaths)
            {
                if (!assetPath.Contains(sourceDir))
                {
                    continue;
                }

                bool foundMatch = false;
                foreach (string path in storagePaths)
                {
                    if (!assetPath.Contains(path))
                    {
                        continue;
                    }

                    foundMatch = true;
                }

                if (foundMatch)
                {
                    continue;
                }

                subAssetPaths.Add(assetPath);
            }
        }

        private void LoadFilteredContent(HashSet<string> subAssetPaths, HashSet<GameObject> prefabs)
        {
            foreach (string path in subAssetPaths)
            {
                GameObject loadedPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (loadedPrefab == null)
                {
                    continue;
                }

                prefabs.Add(loadedPrefab);

                AssetLibraryItem item = items.Find(x => { return x.Replacement == loadedPrefab; });
                if (item != null)
                {
                    continue;
                }

                items.Add(new AssetLibraryItem(loadedPrefab));
            }
        }

        private void SortAndMarkItems(HashSet<GameObject> prefabs)
        {
            foreach (AssetLibraryItem item in items)
            {
                if (item.Replacement == null)
                {
                    continue;
                }

                if (prefabs.Contains(item.Replacement))
                {
                    continue;
                }

                item.MarkInvalid();
            }

            items.Sort((a, b) => { return string.Compare(a.Name, b.Name); });
        }
        #endregion
    }
}

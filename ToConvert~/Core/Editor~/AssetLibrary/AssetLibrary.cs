namespace Mayfair.Core.Editor.AssetLibrary
{
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Extensions;
    using Mayfair.Core.Editor.CustomImportSettings;
    using Mayfair.Core.Editor.EditorService;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class AssetLibrary : EditorService<AssetLibrary, AssetLibraryStorage>, IEditorService
    {
        #region Settings
        [SerializeField, FormerlySerializedAs("fbxImportRules")]
        private FbxImportRules[] assetImportRules =
        {
            new FbxImportRules(CustomImportType.Buildings, ConstsFolders.ART_ROOT,
                               new[]
                               {
                                   "Business",
                                   "Residential",
                                   "Decoration",
                                   "Utility"
                               },
                               new[] {ConstsFolders.ART_EXPORT_ROOT}),
            new FbxImportRules(CustomImportType.Properties, ConstsFolders.ART_ROOT,
                               new[]
                               {
                                   "Map"
                               },
                               new[] {ConstsFolders.ART_EXPORT_ROOT}),
            new FbxImportRules(CustomImportType.PropsArt, ConstsFolders.ART_ROOT,
                               new[]
                               {
                                   "Props",
                               },
                               new[] {ConstsFolders.ART_EXPORT_ROOT}),
            new FbxImportRules(CustomImportType.CustomImportSettings, ConstsFolders.ART_ROOT,
                                new[]
                                {
                                    "Business",
                                    "Residential",
                                    "Decoration"
                                },
                                new string[0])
        };

        [Space]
        [SerializeField]
        private float[] lodDefaultValues = { 30f / 100f, 5f / 100f, 0.005f / 100f, 0.004f / 100f, 0.003f / 100f, 0.002f / 100f };
        #endregion
        protected override double RefreshTimer
        {
            get { return 5; }
        }

        #region Properties
        private static AssetLibrary Instance
        {
            get { return GetValidInstance(AssetLibraryMenu.LOCATION_PATH); }
        }
        #endregion

        #region Class Methods
        public static float[] GetLodValues()
        {
            if (Instance == null)
            {
                return null;
            }

            return Instance.lodDefaultValues;
        }

        public static AssetLibraryItem GetItem(GameObject source)
        {
            return GetItem(source.name);
        }

        public static AssetLibraryItem GetItem(string name)
        {
            if (Instance == null)
            {
                return null;
            }

            foreach (AssetLibraryStorage provider in Instance.providers)
            {
                AssetLibraryItem item = provider.GetItem(name);
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        public static FbxImportRules Get(CustomImportType importType, string importPath)
        {
            if (Instance != null)
            {
                foreach (FbxImportRules rule in Instance.assetImportRules)
                {
                    if (rule.AppliesTo(importType, importPath))
                    {
                        return rule;
                    }
                }
            }

            return null;
        }

        public static GameObject TryReplacing(GameObject original, Transform parent = null)
        {
            AssetLibraryItem item = GetItem(original);
            if (item == null)
            {
                return null;
            }

            if (parent == null)
            {
                parent = original.transform.parent;
            }

            Transform newChild = (PrefabUtility.InstantiatePrefab(item.Replacement, parent) as GameObject).transform;
            newChild.name = item.Replacement.name;
            newChild.CopyLocalValues(original.transform);

            if (!PrefabUtility.IsPartOfPrefabAsset(original))
            {
                original.transform.SetParent(null);
                UnityEngine.Object.DestroyImmediate(original);
            }

            return newChild.gameObject;
        }
        #endregion

        #region IEditorService Members
        public override void LoadContent()
        {
            base.LoadContent();

            ReloadProviders(ConstsFolders.PREFABS);
        }
        #endregion
    }
}


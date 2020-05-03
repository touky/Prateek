namespace Prateek.Core.Editor.Helpers
{
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    public static class AssetMenuExtensions
    {
        #region Class Methods
        ///-------------------------------------------------------------------------
        public static void CreateAsset<T>(MenuCommand menuCommand, string assetName = null, bool doSelect = true)
            where T : MonoBehaviour
        {
            if (string.IsNullOrEmpty(assetName))
            {
                assetName = string.Format("New{0}", typeof(T).Name);
            }

            // Create a custom game object
            var asset = new GameObject(assetName);
            asset.AddComponent<T>();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(asset, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(asset, "Create " + asset.name);

            SelectAsset(asset, doSelect);
        }

        ///-------------------------------------------------------------------------
        public static T CreateAsset<T>(string assetName = null, bool doSelect = true)
            where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            return CreateAsset(asset, assetName, "asset", doSelect);
        }

        ///-------------------------------------------------------------------------
        public static Object CreateAsset(string content, string extension = "txt", string assetName = null, bool doSelect = true)
        {
            var path = GetAssetLocation<TextAsset>(assetName, extension);
            var fileInfo = new FileInfo(path);
            File.WriteAllText(fileInfo.FullName, content);

            AssetDatabase.Refresh();
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

            SelectAsset(asset, doSelect);

            return asset;
        }
    
        ///-------------------------------------------------------------------------
        private static T CreateAsset<T>(T asset, string assetName, string extension, bool doSelect)
            where T : Object
        {
            var path = GetAssetLocation<T>(assetName, extension);

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            SelectAsset(asset, doSelect);

            return asset;
        }

        ///-------------------------------------------------------------------------
        private static string GetAssetLocation<T>(string assetName, string extension)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                assetName = $"My{typeof(T).Name}";
            }

            assetName = $"{assetName}.{extension}";

            var folder = GetPathOfCurrentSelectedObject();

            var path = Path.Combine(folder, assetName);
            return path;
        }

        ///-------------------------------------------------------------------------
        private static void SelectAsset<T>(T asset, bool doSelect)
            where T : Object
        {
            if (doSelect)
            {
                Selection.activeObject = asset;

                EditorUtility.FocusProjectWindow();
            }
        }

        ///-------------------------------------------------------------------------
        public static string GetPathOfCurrentSelectedObject()
        {
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = ConstFolder.ASSETS;
            }
            else if (string.IsNullOrEmpty(Path.GetExtension(path)) == false)
            {
                // If the selected object during created was not a folder then we strip out the filename because we only requir the directory
                path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
            }

            return path;
        }
        #endregion
    }
}

namespace Mayfair.Core.Editor.Utils
{
#if UNITY_EDITOR
    using System.IO;
    using Mayfair.Core.Code.Utils;
    using UnityEditor;
    using UnityEngine;

    public static class AssetMenuHelper
    {
        #region Static and Constants
        public const string ASSET_MENU = ConstsFolders.ASSET + "/Create/Mayfair/";
        public const string GAMEOBJECT_MENU = "GameObject/Mayfair/";
        #endregion

        #region Class Methods
        public static void CreateGameObject<T>(MenuCommand menuCommand, string gameObjectName = "") where T : MonoBehaviour
        {
            if (gameObjectName == string.Empty)
            {
                gameObjectName = string.Format("New{0}", typeof(T).Name);
            }

            // Create a custom game object
            GameObject go = new GameObject(gameObjectName);
            go.AddComponent<T>();

            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);

            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);

            Selection.activeObject = go;
        }

        public static T CreateAsset<T>(string folder = "", string assetName = "", bool doSelect = true) where T : ScriptableObject
        {
            T asset = ScriptableObject.CreateInstance<T>();
            if (assetName == string.Empty)
            {
                assetName = string.Format("My{0}.asset", typeof(T).Name);
            }

            if (string.IsNullOrEmpty(folder))
            {
                folder = GetPathOfCurrentSelectedObject();
            }

            if (!folder.StartsWith(ConstsFolders.ASSET))
            {
                folder = Path.Combine(ConstsFolders.ASSET, folder);
            }

            string root = Application.dataPath.Replace(ConstsFolders.ASSET, string.Empty);
            if (!Directory.Exists(folder))
            {
                string dir = Path.Combine(root, folder);
                DirectoryInfo info = Directory.CreateDirectory(dir);
            }

            string path = Path.Combine(folder, assetName);

            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();

            if (doSelect)
            {
                Selection.activeObject = asset;

                EditorUtility.FocusProjectWindow();
            }

            return asset;
        }

        private static string GetPathOfCurrentSelectedObject()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = ConstsFolders.ASSET;
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
#endif //UNITY_EDITOR
}

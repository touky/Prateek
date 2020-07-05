namespace Mayfair.Core.Editor.Utils
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using UnityEngine.Assertions;
    using UnityEngine.SceneManagement;

    public class CreateGymSceneMenuItem
    {
        #region  Statics and Constants

        // Priority value explanation found here
        // https://blog.redbluegames.com/guide-to-extending-unity-editors-menus-b2de47a746db
        private const int MENU_PRIORITY = 201;

        #endregion

        #region Member Methods

        [MenuItem("Assets/Create/Create Gym Scene", false, MENU_PRIORITY)]
        private static void CreateGymScene()
        {
            Scene gymScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            GameObject loadedObject = LoadLightingAndCameraObject();
            PrefabUtility.InstantiatePrefab(loadedObject);

            EditorSceneManager.SaveScene(gymScene);
        }

        private static GameObject LoadLightingAndCameraObject()
        {
            const string LIGHT_AND_CAMERA_PREFAB = "LightingAndCamera";
            string[] lightingAndCameraAssets = AssetDatabase.FindAssets(LIGHT_AND_CAMERA_PREFAB, new[]
            {
                "Assets/Prefabs/Gym"
            });

            Assert.IsTrue(lightingAndCameraAssets.Length > 0, $"No {LIGHT_AND_CAMERA_PREFAB} found!");
            Assert.AreEqual(1, lightingAndCameraAssets.Length, $"More than one {LIGHT_AND_CAMERA_PREFAB} detected.");

            string assetPath = AssetDatabase.GUIDToAssetPath(lightingAndCameraAssets[0]);
            return AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
        }

        #endregion
    }
}
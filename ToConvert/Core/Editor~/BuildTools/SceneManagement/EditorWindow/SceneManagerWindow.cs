namespace Mayfair.Core.Editor.BuildTools.SceneManagement.EditorWindow
{
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using UnityEditor;
    using UnityEngine;

    public class SceneManagerWindow : EditorWindow
    {
        private static SceneManagerWindow windowInstance;

        private EditorBuildSettingsScene[] gameScenes;
        private SceneSettings sceneSettings;
        private GUID? sceneToRemove;

        private Vector2 scrollPosition = Vector2.zero;

        public static bool IsOpen
        {
            get => windowInstance != null;
        }

        #region Unity Methods

        private void OnGUI()
        {
            using (EditorGUI.ChangeCheckScope check = new EditorGUI.ChangeCheckScope())
            {
                DrawScenesList();
                DrawAddScene();

                if (sceneToRemove != null)
                {
                    RemoveScene();
                }

                if (check.changed)
                {
                    SceneSettings.SaveSceneSettings(sceneSettings);
                }
            }
        }

        #endregion

        [MenuItem("Tools/AutoBuilder/Scene Management")]
        public static void Open()
        {
            windowInstance = GetWindow<SceneManagerWindow>();
            windowInstance.titleContent = new GUIContent("Scene Management");
            windowInstance.Initialize();
            windowInstance.Show();
        }

        public new static void Close()
        {
            if (windowInstance != null)
            {
                ((EditorWindow)windowInstance).Close();
                windowInstance = null;
            }
        }

        private void OnFocus()
        {
            Initialize();
        }

        private void DrawScenesList()
        {
            EditorGUILayout.LabelField("Scenes", EditorStyles.boldLabel);

            using (EditorGUILayout.ScrollViewScope scrollViewScope = new EditorGUILayout.ScrollViewScope(scrollPosition, EditorStyles.helpBox, GUILayout.Width(EditorGUIUtility.currentViewWidth - 5), GUILayout.MinHeight(200)))
            {
                scrollPosition = scrollViewScope.scrollPosition;

                using (new EditorGUILayout.VerticalScope())
                {
                    for (int i = 0; i < sceneSettings.SceneBuildSettings.Count; i++)
                    {
                        DrawSceneReference(sceneSettings.SceneBuildSettings.ElementAt(i).Key);
                    }
                }
            }
        }

        private void DrawSceneReference(GUID sceneGuid)
        {
            const int MIN_FLAGS_WIDTH = 50;
            const int MAX_FLAGS_WIDTH = 100;
            const int BUTTON_WIDTH = 25;
            const int RESERVED_WIDTH = 100 + MAX_FLAGS_WIDTH + BUTTON_WIDTH;

            EditorBuildSettingsScene foundScene = EditorBuildSettings.scenes.FirstOrDefault(scene => scene.guid == sceneGuid);

            if (foundScene == null)
            {
                return;
            }

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - RESERVED_WIDTH;
                    EditorGUILayout.LabelField(foundScene.path);
                    EditorGUIUtility.labelWidth = 0;
                }

                GUILayout.FlexibleSpace();
                DrawBuildTypeDropdown(sceneGuid, MIN_FLAGS_WIDTH, MAX_FLAGS_WIDTH);

                if (GUILayout.Button("-", GUILayout.Width(BUTTON_WIDTH)))
                {
                    sceneToRemove = sceneGuid;
                }
            }
        }

        private void DrawBuildTypeDropdown(GUID sceneGuid, int minWidth, int maxWidth)
        {
            BuildType buildType = sceneSettings.GetBuildType(sceneGuid);
            buildType = (BuildType)EditorGUILayout.EnumFlagsField(buildType, GUILayout.MinWidth(minWidth), GUILayout.MaxWidth(maxWidth));
            sceneSettings.SetBuildType(sceneGuid, buildType);
        }

        private void DrawAddScene()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Add Scene"))
                {
                    string scenePath = EditorUtility.OpenFilePanel("Scene", Application.dataPath, "unity");
                    AddScene(scenePath);
                }
            }
        }

        private void AddScene(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath))
            {
                return;
            }

            Assert.IsTrue(scenePath.Contains(Application.dataPath), "Scene must be one from within this project");

            scenePath = scenePath.Remove(0, Application.dataPath.Length);
            scenePath = $"Assets/{scenePath}";
            EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(scenePath, true);

            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes) { sceneToAdd };
            EditorBuildSettings.scenes = scenes.ToArray();
            sceneSettings.AddScene(sceneToAdd);
        }

        private void RemoveScene()
        {
            List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            scenes.RemoveAt(scenes.FindIndex(scene => scene.guid == sceneToRemove.Value));
            EditorBuildSettings.scenes = scenes.ToArray();
            sceneSettings.RemoveScene(sceneToRemove.Value);
            sceneToRemove = null;
        }

        private void Initialize()
        {
            gameScenes = EditorBuildSettings.scenes;
            sceneSettings = SceneSettings.LoadSceneSettings();
            sceneToRemove = null;

            sceneSettings.SyncScenesWithSettings(gameScenes);
        }
    }
}
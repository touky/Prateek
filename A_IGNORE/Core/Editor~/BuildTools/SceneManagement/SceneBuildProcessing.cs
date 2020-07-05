namespace Mayfair.Core.Editor.BuildTools.SceneManagement
{
    using System.Collections.Generic;
    using EditorWindow;
    using Preprocessing.Interfaces;
    using UnityEditor;
    using UnityEditor.Build.Reporting;
    using UnityEngine;

    public class SceneBuildProcessing : IPreprocessBuild, IPostprocessBuild
    {
        private SceneSettings sceneSettings;
        private bool reOpenSceneManagerWindowAfterBuild;

        public int CallbackOrder
        {
            get => 0;
        }

        private void StripNonRuntimeScenes()
        {
            List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();
            EditorBuildSettingsScene[] projectScenes = EditorBuildSettings.scenes;
            foreach (EditorBuildSettingsScene editorBuildSettingsScene in projectScenes)
            {
                Debug.Assert(sceneSettings.SceneBuildSettings.ContainsKey(editorBuildSettingsScene.guid), "Somehow the sceneSettings got out of sync...");

                if (!sceneSettings.SceneBuildSettings.ContainsKey(editorBuildSettingsScene.guid))
                {
                    Debug.LogError($"Scene Management does not contain an entry for {editorBuildSettingsScene.path} (GUID: {editorBuildSettingsScene.guid})");
                    continue;
                }

                if (sceneSettings.SceneBuildSettings[editorBuildSettingsScene.guid].HasFlag(BuildType.Runtime))
                {
                    buildScenes.Add(editorBuildSettingsScene);
                }
            }

            EditorBuildSettings.scenes = buildScenes.ToArray();
        }

        private void RestoreScenes()
        {
            List<EditorBuildSettingsScene> buildScenes = new List<EditorBuildSettingsScene>();
            foreach (KeyValuePair<GUID, BuildType> keyValuePair in sceneSettings.SceneBuildSettings)
            {
                string scenePath = AssetDatabase.GUIDToAssetPath(keyValuePair.Key.ToString());
                EditorBuildSettingsScene scene = new EditorBuildSettingsScene(scenePath, true);
                buildScenes.Add(scene);
            }

            EditorBuildSettings.scenes = buildScenes.ToArray();
        }

        #region IPostprocessBuild

        public void OnPostprocessBuild(BuildSummary buildSummary)
        {
            sceneSettings = SceneSettings.LoadSceneSettings();
            RestoreScenes();

            if (reOpenSceneManagerWindowAfterBuild)
            {
                SceneManagerWindow.Open();
            }
        }

        #endregion

        #region IPreprocessBuild

        public void OnPreprocessBuild(BuildTarget buildTarget)
        {
            // We close the window so that we do not have to compete with it trying to update the scenes
            // it is tracking. If it is open, it could make out temporary scene modifications permanent.
            reOpenSceneManagerWindowAfterBuild = SceneManagerWindow.IsOpen;
            SceneManagerWindow.Close();

            sceneSettings = SceneSettings.LoadSceneSettings();
            StripNonRuntimeScenes();
        }

        #endregion
    }
}
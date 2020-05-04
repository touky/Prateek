namespace Mayfair.Core.Editor.BuildTools.SceneManagement
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using UnityEditor;
    using UnityEditor.VersionControl;
    using UnityEngine;

    [Serializable]
    public class SceneSettings : ISerializationCallbackReceiver
    {
        private const string SCENE_SETTINGS_RELATIVE_PATH = "/SceneSettings.json";

        [SerializeField]
        private List<string> guids;

        [SerializeField]
        private List<BuildType> buildTypes;

        [NonSerialized]
        private Dictionary<GUID, BuildType> sceneBuildSettings;

        public IReadOnlyDictionary<GUID, BuildType> SceneBuildSettings
        {
            get => sceneBuildSettings;
        }

        public SceneSettings()
        {
            sceneBuildSettings = new Dictionary<GUID, BuildType>();
            guids = new List<string>();
            buildTypes = new List<BuildType>();
        }

        public static SceneSettings LoadSceneSettings()
        {
            string settingsPath = Application.dataPath + SCENE_SETTINGS_RELATIVE_PATH;

            if (File.Exists(settingsPath))
            {
                string sceneSettingsJson = File.ReadAllText(settingsPath);
                return JsonUtility.FromJson<SceneSettings>(sceneSettingsJson);
            }

            return new SceneSettings();
        }

        public static void SaveSceneSettings(SceneSettings sceneSettings)
        {
            string settingsPath = Application.dataPath + SCENE_SETTINGS_RELATIVE_PATH;

            CheckoutOrAdd(settingsPath);

            string sceneSettingsJson = JsonUtility.ToJson(sceneSettings);
            File.WriteAllText(settingsPath, sceneSettingsJson);
        }

        public static void CheckoutOrAdd(string path)
        {
            if (!File.Exists(path) && !Directory.Exists(path))
            {
                return;
            }

            Asset asset = Provider.GetAssetByPath($"Assets{SCENE_SETTINGS_RELATIVE_PATH}");
            if (asset != null)
            {
                Task task = null;
                if (Provider.CheckoutIsValid(asset))
                {
                    task = Provider.Checkout(asset, CheckoutMode.Asset);
                }
                else
                {
                    task = Provider.Add(asset, true);
                }

                task.Wait();
            }
        }

        public void SyncScenesWithSettings(EditorBuildSettingsScene[] gameScenes)
        {
            AddMissingSettings(gameScenes);
            RemoveExcessSettings(gameScenes);
        }

        public void AddScene(EditorBuildSettingsScene scene)
        {
            if (sceneBuildSettings.ContainsKey(scene.guid))
            {
                Debug.Log("[SceneSettings] - Scene is already being tracked.");
                return;
            }

            sceneBuildSettings.Add(scene.guid, BuildType.None);
        }

        public void RemoveScene(GUID guid)
        {
            sceneBuildSettings.Remove(guid);
        }

        public BuildType GetBuildType(GUID guid)
        {
            return sceneBuildSettings[guid];
        }

        public void SetBuildType(GUID guid, BuildType buildType)
        {
            sceneBuildSettings[guid] = buildType;
        }

        private void AddMissingSettings(EditorBuildSettingsScene[] gameScenes)
        {
            foreach (EditorBuildSettingsScene gameScene in gameScenes)
            {
                if (!sceneBuildSettings.ContainsKey(gameScene.guid))
                {
                    sceneBuildSettings.Add(gameScene.guid, BuildType.Runtime);
                }
            }
        }

        private void RemoveExcessSettings(EditorBuildSettingsScene[] gameScenes)
        {
            List<GUID> settingsToRemove = new List<GUID>();
            foreach (KeyValuePair<GUID, BuildType> sceneBuildSetting in sceneBuildSettings)
            {
                if (!DoesSceneExist(sceneBuildSetting.Key, gameScenes))
                {
                    settingsToRemove.Add(sceneBuildSetting.Key);
                }
            }

            foreach (GUID guid in settingsToRemove)
            {
                sceneBuildSettings.Remove(guid);
            }
        }

        private bool DoesSceneExist(GUID guid, EditorBuildSettingsScene[] gameScenes)
        {
            foreach (EditorBuildSettingsScene scene in gameScenes)
            {
                if (scene.guid == guid)
                {
                    return true;
                }
            }

            return false;
        }

        #region ISerializationCallbackReceiver

        public void OnBeforeSerialize()
        {
            guids.Clear();
            buildTypes.Clear();

            foreach (KeyValuePair<GUID, BuildType> buildSetting in sceneBuildSettings)
            {
                guids.Add(buildSetting.Key.ToString());
                buildTypes.Add(buildSetting.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            if (guids.Count != buildTypes.Count)
            {
                Debug.LogError("SceneSettings data mismatch. Settings will need to be reset in the SceneManagerWindow.");
                guids.Clear();
                buildTypes.Clear();
                return;
            }

            for (int i = 0; i < guids.Count; i++)
            {
                sceneBuildSettings.Add(new GUID(guids[i]), buildTypes[i]);
            }
        }

        #endregion
    }
}
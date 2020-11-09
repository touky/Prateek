namespace Prateek.Editor.Settings
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.FrameworkSettings;
    using Prateek.Runtime.Core.Helpers.Files;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    internal class PrateekSettingsProvider : SettingsProvider
    {
        #region Fields
        private bool showFrameworkSettings = false;
        private Dictionary<Type, FrameworkSettings> frameworkSettings = new Dictionary<Type, FrameworkSettings>();
        #endregion

        #region Constructors
        public PrateekSettingsProvider(string path, SettingsScope scope = SettingsScope.User)
            : base(path, scope) { }
        #endregion

        #region Unity EditorOnly Methods
        public override void OnGUI(string searchContext)
        {
            DrawSettingsPrompt("Prateek Main Settings", PrateekSettings.Instance != null, () => PrateekSettings.CreateSettings());
            if (!PrateekSettings.Instance)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            using (new EditorGUI.IndentLevelScope())
            {
                PrateekSettings.OnGUI(searchContext);
            }

            if (FrameworkSettingsForagerWorker.Instance == null)
            {
                return;
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Framework Custom Settings", EditorStyles.boldLabel);

            using (new EditorGUI.IndentLevelScope())
            {
                foreach (var foundType in FrameworkSettingsForagerWorker.Instance.FoundTypes)
                {
                    if (!frameworkSettings.ContainsKey(foundType))
                    {
                        frameworkSettings.Add(foundType, Activator.CreateInstance(foundType) as FrameworkSettings);
                    }

                    var frameworkSetting = frameworkSettings[foundType];
                    var assetPath        = $"{ConstFolder.ASSETS_RESOURCES}/{frameworkSetting.DefaultPath}{ConstExtension.ASSET}";
                    var settingFile      = AssetDatabase.LoadAssetAtPath(assetPath, frameworkSetting.ResourceType);
                    DrawSettingsPrompt($"{foundType.Name}", settingFile != null, () => Create(frameworkSetting, assetPath));
                }
            }
        }
        #endregion

        #region Class Methods
        public static bool IsSettingsAvailable()
        {
            return true;
        }

        /// This function is called when the user clicks on the MyCustom element in the Settings window.
        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            if (PrateekSettings.Instance != null)
            {
                PrateekSettings.InitGUI();
            }
        }

        private void DrawSettingsPrompt(string text, bool exists, Action createAction)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField($"{(exists ? "[OK]" : "[NOT FOUND]")} {text}");
                if (exists)
                {
                    return;
                }

                if (GUI.Button(EditorGUILayout.GetControlRect(), "Create settings file"))
                {
                    createAction();
                }
            }
        }

        private void Create(FrameworkSettings settings, string assetPath)
        {
            var resource = ScriptableObject.CreateInstance(settings.ResourceType);

            //todo Merge
            DirectoryHelper.DirectoryMustExist(Path.GetDirectoryName(assetPath));
            AssetDatabase.CreateAsset(resource, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        ///     Register the SettingsProvider
        /// </summary>
        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            if (IsSettingsAvailable())
            {
                var provider = new PrateekSettingsProvider($"{ConstFolder.PROJECT}/{ObjectNames.NicifyVariableName(nameof(PrateekSettings))}", SettingsScope.Project);

                // Automatically extract all keywords from the Styles.
                provider.keywords = GetSearchKeywordsFromGUIContentProperties<PrateekSettings.SearchableStyles>();
                return provider;
            }

            return null;
        }
        #endregion
    }
}




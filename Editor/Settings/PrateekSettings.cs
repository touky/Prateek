namespace Prateek.Editor.Settings
{
    using System.IO;
    using Prateek.Runtime.Core.Consts;
    using Prateek.Runtime.Core.Helpers.Files;
    using UnityEditor;
    using UnityEngine;

    internal class PrateekSettings : ScriptableObject
    {
        #region Static and Constants
        public static readonly string ASSET_PATH = $"{ConstFolder.PRATEEK_EDITOR_SETTINGS}/{nameof(PrateekSettings)}{ConstExtension.ASSET}";

        private const string PROPERTY_RECOMPILE = "stopPlayModeOnScriptCompile";

        private static PrateekSettings instance;
        private static SerializedObject serializedObject;
        #endregion

        #region Settings
        [SerializeField]
        private bool stopPlayModeOnScriptCompile = true;
        #endregion

        #region Properties
        internal static bool StopPlayModeOnScriptCompile { get { return Instance == null ? false : Instance.stopPlayModeOnScriptCompile; } }

        internal static PrateekSettings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = GetSettings();
                }

                return instance;
            }
        }
        #endregion

        #region Unity EditorOnly Methods
        internal static void OnGUI(string searchContext)
        {
            if (serializedObject == null)
            {
                return;
            }

            // Use IMGUI to display UI:
            var recompileProperty = serializedObject.FindProperty(PROPERTY_RECOMPILE);
            recompileProperty.boolValue = EditorGUILayout.ToggleLeft(SearchableStyles.recompile, recompileProperty.boolValue);

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region Class Methods
        private void Reset()
        {
            stopPlayModeOnScriptCompile = true;
        }

        private static PrateekSettings GetSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<PrateekSettings>(ASSET_PATH);
            return settings;
        }

        internal static PrateekSettings CreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<PrateekSettings>(ASSET_PATH);
            if (settings == null)
            {
                settings = CreateInstance<PrateekSettings>();
                settings.Reset();

                //todo Merge
                DirectoryHelper.DirectoryMustExist(Path.GetDirectoryName(ASSET_PATH));
                AssetDatabase.CreateAsset(settings, ASSET_PATH);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        internal static void InitGUI()
        {
            if (Instance == null)
            {
                return;
            }

            serializedObject = new SerializedObject(Instance);
        }
        #endregion

        #region Nested type: SearchableStyles
        internal class SearchableStyles
        {
            #region Static and Constants
            public static GUIContent recompile = new GUIContent(ObjectNames.NicifyVariableName(PROPERTY_RECOMPILE));
            #endregion
        }
        #endregion
    }
}

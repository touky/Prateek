namespace Mayfair.Core.Editor.BuildTools.Versioner
{
    using UnityEditor;
    using UnityEngine;

    public class VersionerEditor : EditorWindow
    {
        [MenuItem("Tools/Versioner")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(VersionerEditor), true, "Versioner");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Version", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            {
                EditorGUILayout.LabelField(PlayerSettings.bundleVersion);
            }
            EditorGUI.indentLevel--;

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Increment Major"))
                {
                    Versioner.IncrementMajor();
                }
                if (GUILayout.Button("Increment Minor"))
                {
                    Versioner.IncrementMinor();
                }
                if (GUILayout.Button("Increment Patch"))
                {
                    Versioner.IncrementPatch();
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUIStyle warningStyle = new GUIStyle(EditorStyles.label)
            {
                richText = true,
                wordWrap = true
            };

            string warning = Versioner.GetVersionWarning();
            if (!string.IsNullOrEmpty(warning))
            {
                EditorGUILayout.LabelField(string.Format("<color=red><b>Warning</b>: {0}</color>", warning), warningStyle);
            }
        }
    }
}

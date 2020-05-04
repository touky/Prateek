namespace Mayfair.Core.Editor.GUI
{
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    public static class GUIHelper
    {
        #region Methods
        public static void AllowFocus(Object focus)
        {
            AllowFocus(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(80)), focus);
        }

        public static void AllowFocus(Rect rect, Object focus)
        {
            if (GUI.Button(rect, ConstsEditor.FOCUS))
            {
                EditorGUIUtility.PingObject(focus);
            }
        }

        public static void ShowStatusBox(bool exist, params GUILayoutOption[] options)
        {
            ShowStatusBox(exist, string.Empty, options: options);
        }

        public static void ShowStatusBox(Rect rect, bool exist)
        {
            ShowStatusBox(rect, exist, string.Empty);
        }

        public static void ShowStatusBox(Color color, string text, params GUILayoutOption[] options)
        {
            Rect rect = EditorGUILayout.GetControlRect(options);
            ShowStatusBox(rect, color, string.Empty, text);
        }

        public static void ShowStatusBox(Rect rect, Color color, string text, params GUILayoutOption[] options)
        {
            ShowStatusBox(rect, color, string.Empty, text);
        }

        public static void ShowStatusBox(bool exist, string boxName, string foundText = "FOUND", string missingText = "MISSING", GUIStyle style = null, params GUILayoutOption[] options)
        {
            Rect rect = EditorGUILayout.GetControlRect(options);
            ShowStatusBox(rect, exist, boxName, foundText, missingText, style);
        }

        public static void ShowStatusBox(Rect rect, bool exist, string boxName, string foundText = "FOUND", string missingText = "MISSING", GUIStyle style = null)
        {
            ShowStatusBox(rect, exist ? Color.green : Color.red, boxName, exist ? foundText : missingText, style);
        }

        public static void ShowStatusBox(Rect rect, Color color, string boxName, string text, GUIStyle style = null)
        {
            if (style == null)
            {
                style = GUIStyleHelper.CreateStyle("ShowStatusBox", Color.white, Color.grey, Color.white);
            }

            using (new ColorScope(color))
            {
                EditorGUI.LabelField(rect, boxName + text, style);
            }
        }
        #endregion
    }
}

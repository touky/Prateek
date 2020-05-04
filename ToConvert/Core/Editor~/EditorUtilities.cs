namespace Mayfair.Core.Editor
{
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    [InitializeOnLoad]
    public static class EditorUtilities
    {
        #region Consts
        #endregion

        
        static EditorUtilities()
        {
            EditorApplication.update += Update;
        }

        #region Properties
        public static bool IsFocused { get; private set; }
        #endregion

        #region Class Methods
        private static void Update()
        {
            IsFocused = InternalEditorUtility.isApplicationActive;
        }

        public static string DrawSelectablePathUi(string label, string path)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(150));
                EditorGUILayout.TextField(path);

                if (GUILayout.Button("Choose Folder", GUILayout.Width(100)))
                {
                    path = EditorUtility.OpenFolderPanel("Select path", path, string.Empty);
                }
            }
            EditorGUILayout.EndHorizontal();

            return path;
        }
        #endregion
    }
}

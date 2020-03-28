namespace Mayfair.Core.Editor.Service
{
    using Mayfair.Core.Code.Service;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(GameInitActivator))]
    public class GameInitActivatorEditor : Editor
    {
        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameInitActivator activator = target as GameInitActivator;

            activator.name = typeof(GameInitActivator).Name;

            if (activator.enabled && !EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Order of initialization:");
                using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                {
                    for (int childIndex = 0; childIndex < activator.transform.childCount; childIndex++)
                    {
                        Transform child = activator.transform.GetChild(childIndex);

                        if (child.gameObject.activeSelf)
                        {
                            child.gameObject.SetActive(false);
                            EditorUtility.SetDirty(child);
                        }
                        EditorGUILayout.LabelField(child.name);
                    }
                }
            }
        }
        #endregion
    }
}

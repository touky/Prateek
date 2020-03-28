namespace Mayfair.Core.Editor.AssetLibrary
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;

    [CustomEditor(typeof(AssetLibrary))]
    public class AssetLibraryEditor : Editor
    {
        #region Fields
        private bool providerFoldout = false;
        private ReflectedField<List<AssetLibraryStorage>> providers = "providers";
        #endregion

        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            AssetLibrary library = target as AssetLibrary;
            library.TryReloadContent();

            providers.Init(library);

            EditorGUILayout.Space();
            providerFoldout = EditorGUILayout.Foldout(providerFoldout, "< Found asset library storage >", true);
            if (providerFoldout)
            {
                using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
                {
                    foreach (AssetLibraryStorage provider in providers.Value)
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            EditorGUILayout.LabelField(provider.name);
                            EditorGUILayout.LabelField(AssetDatabase.GetAssetPath(provider));
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
        #endregion
    }
}

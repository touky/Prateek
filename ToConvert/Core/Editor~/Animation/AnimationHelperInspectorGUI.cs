namespace Mayfair.Core.Editor.Animation
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    [InitializeOnLoad]
    internal static class AnimationHelperInspectorGUI
    {
        #region Static and Constants
        private const string FOLDOUT_MAIN = "MainFoldout";

        private static EditorPrefsWrapper prefsWrapper;
        private static AnimationHelperContext context;
        #endregion

        #region Constructors
        static AnimationHelperInspectorGUI()
        {
            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }
        #endregion

        #region Class Methods
        private static void OnPostHeaderGUI(Editor editor)
        {
            AnimationClip clip = editor.target as AnimationClip;
            if (clip == null)
            {
                return;
            }

            if (prefsWrapper == null)
            {
                prefsWrapper = new EditorPrefsWrapper(typeof(AnimationHelperInspectorGUI).Name);
            }

            if (context != null && context.target != clip)
            {
                context = null;
            }

            bool foldout = false;
            prefsWrapper.Get(FOLDOUT_MAIN, ref foldout, foldout);
            using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
            {
                foldout = EditorGUILayout.Foldout(foldout, "Show animation binding Editor", true);
                if (scope.changed)
                {
                    prefsWrapper.Set(FOLDOUT_MAIN, foldout);
                }

                if (!foldout)
                {
                    return;
                }
            }

            if (context == null)
            {
                context = new AnimationHelperContext(clip);
            }

            if (context.bindingPaths.Count == 0)
            {
                context.Init();
            }

            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                List<string> keys = new List<string>(context.bindingPaths.Keys);
                keys.Sort();
                foreach (string key in keys)
                {
                    string value = context.bindingPaths[key];
                    string path = AnimationHelperContext.TrimPath(key);

                    using (EditorGUI.ChangeCheckScope scope = new EditorGUI.ChangeCheckScope())
                    {
                        using (new EditorGUILayout.HorizontalScope())
                        {
                            value = EditorGUILayout.TextField(value);
                            EditorGUILayout.LabelField(path);
                        }

                        if (scope.changed)
                        {
                            context.bindingPaths[key] = value;
                            context.bindingModified = true;
                        }
                    }
                }

                if (GUI.Button(EditorGUILayout.GetControlRect(), "Clear any modification"))
                {
                    context.bindingPaths.Clear();
                }

                using (new EditorGUI.DisabledScope(!context.bindingModified))
                {
                    if (GUI.Button(EditorGUILayout.GetControlRect(), "Apply modifications"))
                    {
                        context.applyModifications = true;
                    }

                    EditorGUILayout.Space();
                    foreach (KeyValuePair<string, string> pair in context.bindingPaths)
                    {
                        string newPath = AnimationHelperContext.ConvertPath(pair.Key, pair.Value);
                        if (newPath == pair.Key)
                        {
                            continue;
                        }

                        EditorGUILayout.LabelField($"Original: {pair.Key}");
                        EditorGUILayout.LabelField($"     New: {newPath}");
                    }
                }

                if (Event.current.type == EventType.Repaint && context.applyModifications)
                {
                    context.applyModifications = false;

                    context.ApplyPathChanges();

                    //Stop there and re-init the editor
                    context = null;
                }
            }
        }
        #endregion
    }

    internal class AnimationBindingsSetup
    {
        #region Fields
        public EditorCurveBinding binding;
        public AnimationCurve curve;
        public List<string> splitPath = new List<string>();
        #endregion
    }
}

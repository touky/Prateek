namespace Mayfair.Core.Editor.AssetLibrary
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Utils.Debug.Reflection;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.GUI;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(AssetLibraryStorage))]
    public class AssetLibraryStorageEditor : Editor
    {
        private const int COLUMN_COUNT = 5;

        #region Fields
        private List<float> tagsSizes = new List<float>(COLUMN_COUNT);
        private List<float> realSizes = new List<float>(COLUMN_COUNT);
        private ReflectedField<List<AssetLibraryItem>> items = "items";
        private ReflectedField<GameObject> replacement = "replacement";
        #endregion

        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            AssetLibraryStorage storage = target as AssetLibraryStorage;
            items.Init(storage);

            storage.TryReloadContent();

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Found item for this library storage");
                if (GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(300)), "Remove all invalids"))
                {
                    storage.Cleanup();
                }
            }

            foreach (AssetLibraryItem item in items.Value)
            {
                CalculateSize(item);
            }

            using (new EditorGUI.IndentLevelScope(ConstsEditor.ONE_INDENT))
            {
                foreach (AssetLibraryItem item in items.Value)
                {
                    Rect lineRect = EditorGUILayout.GetControlRect();
                    int r = 0;
                    Rect[] rects = RectHelper.SplitX(ref lineRect, Split.FixedSize(30), 2, 2, 3, Split.FixedSize(80));
                    {
                        using (EditorGUI.ChangeCheckScope changeScope = new EditorGUI.ChangeCheckScope())
                        {
                            item.Enabled = EditorGUI.ToggleLeft(rects[r++], GUIContent.none, item.Enabled);
                            if (changeScope.changed)
                            {
                                EditorUtility.SetDirty(this);
                            }
                        }

                        replacement.Init(item);
                        bool replacementIsNull = item.Replacement == null;
                        using (new EditorGUI.DisabledScope(!item.Enabled))
                        {
                            EditorGUI.LabelField(rects[r++], item.Name);

                            using (EditorGUI.ChangeCheckScope changeScope = new EditorGUI.ChangeCheckScope())
                            {
                                replacement.Value = EditorGUI.ObjectField(rects[r++], replacement.Value, typeof(GameObject), false) as GameObject;
                                if (changeScope.changed)
                                {
                                    EditorUtility.SetDirty(this);
                                }
                            }

                            DrawFoundTags(rects[r++], item);

                            using (new EditorGUI.DisabledScope(replacementIsNull))
                            {
                                if (GUI.Button(rects[r++], "Remove"))
                                {
                                    storage.Cleanup(item);
                                }
                            }
                        }
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        public void CalculateSize(AssetLibraryItem libraryItem)
        {
            List<string> tags = libraryItem.Tags;
            for (int t = 0; t < tags.Count; t++)
            {
                if (t >= tagsSizes.Count)
                {
                    tagsSizes.Add(0);
                }

                string tag = tags[t];
                tagsSizes[t] = Mathf.Max(tagsSizes[t], tag.Length);
            }
        }

        public void DrawFoundTags(Rect rect, AssetLibraryItem libraryItem)
        {
            realSizes.Clear();
            realSizes.AddRange(tagsSizes);
            for (int r = 0; r < realSizes.Count; r++)
            {
                realSizes[r] = realSizes[r] * 12;
            }

            List<string> tags = libraryItem.Tags;
            if (libraryItem.Replacement != null)
            {
                Rect[] rects = RectHelper.SplitX(ref rect, realSizes);
                for (int t = 0; t < tags.Count; t++)
                {
                    string tag = tags[t];
                    float degree = 13 * (360 + tag.GetHashCode() % 360) % 360;
                    GUIHelper.ShowStatusBox(rects[t], Color.HSVToRGB(ColorHelper.DegreeToHue(degree), 0.4f, 1), tag);
                }
            }
            else
            {
                GUIHelper.ShowStatusBox(rect, Color.red, "REPLACEMENT MISSING");
            }
        }
        #endregion
    }
}

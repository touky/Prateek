namespace Mayfair.Core.Editor.Addressables
{
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.Protocol;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEditor.AddressableAssets;
    using UnityEditor.AddressableAssets.Settings;
    using UnityEngine;

    [InitializeOnLoad]
    internal static class AddressableAssetInspectorGUI
    {
        #region Static and Constants
        private const string KEYWORDS = "KEYWORDS";
        private static float[] GUISizes = {-60, -10, -110, -2, -4, 120};

        private static GUIContent titleContent;
        private static GUIContent noMultipleContent;

        private static List<string> keywords = null;
        private static int selectedKeyword = Consts.INDEX_NONE;
        private static Object lastTarget;
        #endregion

        #region Constructors
        static AddressableAssetInspectorGUI()
        {
            InitGUI();
            InitKeywords();

            Editor.finishedDefaultHeaderGUI += OnPostHeaderGUI;
        }
        #endregion

        #region Class Methods
        private static void OnPostHeaderGUI(Editor editor)
        {
            if (editor.target is AssetImporter)
            {
                return;
            }

            AddressableAssetSettings addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
            if (addressableSettings == null)
            {
                return;
            }

            bool applyAllAddress = false;
            if (editor.targets.Length > 1)
            {
                applyAllAddress = GUI.Button(EditorGUILayout.GetControlRect(), "Apply ALL");
                EditorGUILayout.Space();
            }

            foreach (Object target in editor.targets)
            {
                AddressableAssetEntry entry = UnityAddressablesHelper.FindEntry(addressableSettings, target);
                if (entry == null)
                {
                    continue;
                }

                DrawTarget(target, entry, applyAllAddress);

                if (editor.targets.Length > 1)
                {
                    EditorGUILayout.Space();
                }
            }
        }

        private static void DrawTarget(Object target, AddressableAssetEntry entry, bool applyAllAddress)
        {
            if (!target.GetType().IsSubclassOf(typeof(AssetImporter)))
            {
                if (lastTarget != target)
                {
                    selectedKeyword = Consts.INDEX_NONE;
                }

                lastTarget = target;
            }

            GUILayout.Label(titleContent);

            if (selectedKeyword <= Consts.INDEX_NONE)
            {
                for (int k = 0; k < keywords.Count; k++)
                {
                    if (entry.address.StartsWith(keywords[k]))
                    {
                        selectedKeyword = k;
                        break;
                    }
                }
            }

            Rect mainRect = EditorGUILayout.GetControlRect();

            //Spend an empty line because we'll use the GUISizes to ensure the same exact positions
            EditorGUILayout.GetControlRect();

            bool applyAddress = false;
            string realName = string.Empty;
            string newAddress = string.Empty;
            string newGroup = string.Empty;
            Rect[] rects = RectHelper.SplitX(ref mainRect, GUISizes);
            for (int pass = 0; pass < Consts.TWO_PASS; pass++)
            {
                int index = Consts.THIRD_ITEM;
                using (new EditorGUILayout.HorizontalScope())
                {
                    switch (pass)
                    {
                        case Consts.FIRST_PASS:
                        {
                            applyAddress = applyAllAddress || GUI.Button(rects[Consts.FIRST_ITEM], "Apply");

                            selectedKeyword = EditorGUI.Popup(rects[index++], selectedKeyword, keywords.ToArray());
                            index++;
                            EditorGUI.LabelField(rects[index++], "/");

                            if (target is AssetImporter)
                            {
                                realName = Path.GetFileNameWithoutExtension((target as AssetImporter).assetPath);
                                EditorGUI.LabelField(rects[index++], realName);
                            }
                            else if (target.name != string.Empty)
                            {
                                realName = target.name;
                                EditorGUI.LabelField(rects[index++], realName);
                            }
                            else
                            {
                                applyAddress = false;
                                EditorGUI.LabelField(rects[index++], "/!\\ WARNING /!\\ Target name is empty, warns benjaminh !!!!");
                            }

                            if (selectedKeyword <= Consts.INDEX_NONE || selectedKeyword >= keywords.Count)
                            {
                                applyAddress = false;
                            }
                            else
                            {
                                newAddress = $"{keywords[selectedKeyword]}/{realName}";
                            }

                            break;
                        }
                        case Consts.SECOND_PASS:
                        {
                            if (selectedKeyword <= Consts.INDEX_NONE || selectedKeyword >= keywords.Count)
                            {
                                newGroup = "--";
                            }
                            else
                            {
                                newGroup = AddressableHelper.GetGroupFromName(realName, keywords[selectedKeyword]);
                            }

                            EditorGUI.LabelField(rects[index++], "Will save in group:");
                            index += Consts.TWO_ITEMS;
                            EditorGUI.LabelField(rects[index++], newGroup);
                            break;
                        }
                    }
                }

                for (int r = 0; r < rects.Length; r++)
                {
                    Rect rect = rects[r];
                    rect.y += rect.height;
                    rects[r] = rect;
                }
            }

            EditorGUILayout.Space();

            if (applyAddress)
            {
                AddressableHelper.SetAddress(target, newAddress, newGroup);
            }
        }

        private static void InitGUI()
        {
            titleContent = new GUIContent("Mayfair Addressable Suggestion", "Use this tooltip to properly setup your address");
            noMultipleContent = new GUIContent("Mayfair Addressable Suggestion: Multiple Targets not supported");
        }

        private static void InitKeywords()
        {
            keywords = new List<string>();

            List<Assembly> assemblies = AssemblyHelper.GetAssembliesMatching(ProtocolHelper.ASSEMBLY_MATCH[0], ProtocolHelper.ASSEMBLY_MATCH[1]);
            foreach (Assembly assembly in assemblies)
            {
                foreach (TypeInfo typeInfo in assembly.DefinedTypes)
                {
                    FieldInfo field = typeInfo.GetDeclaredField(KEYWORDS);
                    if (field != null && field.FieldType.IsArray)
                    {
                        string[] foundKeywords = (string[]) field.GetValue(null);
                        keywords.AddRange(foundKeywords);
                    }
                }
            }

            for (int k = 0; k < keywords.Count; k++)
            {
                keywords[k] = Code.Utils.Helpers.PathHelper.RemoveLeadingAndTrailingSlashes(keywords[k]);
            }
        }
        #endregion
    }
}

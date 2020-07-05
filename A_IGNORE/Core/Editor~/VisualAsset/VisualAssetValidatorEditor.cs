namespace Mayfair.Core.Editor.VisualAsset
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.TagSystem;
    using Mayfair.Core.Code.Utils;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Code.Utils.Types.UniqueId;
    using Mayfair.Core.Code.VisualAsset;
    using Mayfair.Core.Editor.Animation;
    using Mayfair.Core.Editor.FBXExporter;
    using Mayfair.Core.Editor.GUI;
    using Mayfair.Core.Editor.ObjectCategorizing;
    using Mayfair.Core.Editor.Utils;
    using UnityEditor;
    using UnityEngine;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(VisualAssetValidator))]
    public class VisualAssetValidatorEditor : Editor
    {
        #region FileInfoMode enum
        private enum FileInfoMode
        {
            RegenerateButton,
            MainUse
        }
        #endregion

        #region PassMode enum
        private enum PassMode
        {
            Main,
            Animation
        }
        #endregion

        #region Static and Constants
        private const string TAG_PLACEMENT = "Placement";
        private const string TAG_SELECTION = "Selection";
        private const string PREVIEW = "__preview__";
        private const int GENERATE_BUTTON_SIZE = 180;
        private const int PADDING = 3;
        #endregion

        #region Fields
        private bool fbxFoldout = false;

        private double lastDrawnLineRebuild = 0;
        private double drawnLineRebuildDuration = 5;
        private List<DrawnLine> drawnLines = new List<DrawnLine>();
        private UniqueId nonAdditiveId = string.Empty;
        #endregion

        #region Properties
        public UniqueId NonAdditiveId
        {
            get
            {
                if (nonAdditiveId.Type == UniqueIdType.None)
                {
                    nonAdditiveId = new UniqueId();
                    nonAdditiveId.AddTag(AssemblyHelper.GetTypeFromAnyAssembly(TAG_PLACEMENT));
                    nonAdditiveId.AddTag(AssemblyHelper.GetTypeFromAnyAssembly(TAG_SELECTION));
                }

                return nonAdditiveId;
            }
        }
        #endregion

        #region Class Methods
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (targets.Length == 1)
            {
                DrawTarget(serializedObject, target);
            }
            else
            {
                Rect buttonRect = EditorGUILayout.GetControlRect();
                bool oldGUIEnabled = GUI.enabled;
                GUI.enabled = true;
                {
                    DrawButton(ref buttonRect, "Generate ALL targets", () =>
                    {
                        foreach (UnityEngine.Object target in targets)
                        {
                            string path = AssetDatabase.GetAssetPath(target);
                            FbxExporterEditorWindow.ExportLODs(path);
                        }
                    });
                }
                GUI.enabled = oldGUIEnabled;

                foreach (UnityEngine.Object target in targets)
                {
                    DrawTarget(new SerializedObject(target), target);
                }
            }
        }

        public void DrawTarget(SerializedObject serializedObject, UnityEngine.Object target)
        {
            if (drawnLines.Count == 0 || EditorApplication.timeSinceStartup > lastDrawnLineRebuild + drawnLineRebuildDuration)
            {
                lastDrawnLineRebuild = EditorApplication.timeSinceStartup;
                drawnLines.Clear();

                DoDrawTarget(serializedObject, target);
            }

            DrawLines();
        }

        public void DoDrawTarget(SerializedObject serializedObject, UnityEngine.Object target)
        {
            serializedObject.Update();

            VisualAssetValidator assetData = target as VisualAssetValidator;

            //Retrieve lod roots
            List<CategorizedInstance> categorizedInstances = new List<CategorizedInstance>();
            GameObjectCategorizer.Gather(assetData.gameObject, categorizedInstances);
            GameObjectCategorizer.Identify(categorizedInstances);

            List<CategoryContentType> lodRequired = new List<CategoryContentType>(VisualAssetBuilderHelper.PotentialLods);
            List<CategorizedInstance> lodRoots = new List<CategorizedInstance>();
            CategorizerManipulator.GatherLodRoot(categorizedInstances, lodRoots);
            for (int r = 0; r < lodRequired.Count; r++)
            {
                if (r < lodRoots.Count)
                {
                    continue;
                }

                lodRequired[r] |= CategoryContentType.EXTRA;
            }

            GUILayoutOption statusBoxOption = GUILayout.MaxWidth(120);

            string originalPath = AssetDatabase.GetAssetPath(assetData);
            string assetPath = originalPath;
            if (string.IsNullOrEmpty(assetPath))
            {
                VisualAssetValidator prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(assetData);
                assetPath = AssetDatabase.GetAssetPath(prefabSource);
            }

            if (assetPath == string.Empty)
            {
                return;
            }

            FileInfo assetInfo = new FileInfo(assetPath);

            DrawMainAsset(assetInfo, lodRequired, assetPath, originalPath, statusBoxOption,
                          () =>
                          {
                              string path = AssetDatabase.GetAssetPath(target);
                              FbxExporterEditorWindow.ExportLODs(path);
                          },
                          pass =>
                          {
                              string path = AssetDatabase.GetAssetPath(target);
                              path = string.Format(FbxExporterEditorWindow.PASS_FORMAT, pass, path);
                              FbxExporterEditorWindow.ExportLODs(path);
                          });

            DrawAnimations(assetInfo, lodRequired, assetPath, originalPath, statusBoxOption);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAnimations(FileInfo assetInfo, List<CategoryContentType> lodRequired, string assetPath, string originalPath, GUILayoutOption statusBoxOption)
        {
            string[] foundAnimations = AnimationBuilderHelper.GetAnimationsForAsset(originalPath);
            if (foundAnimations.Length == 0)
            {
                return;
            }

            string originalFolder = Path.GetDirectoryName(originalPath);
            string animationFolder = AnimationBuilderHelper.GetAnimationFolder(originalFolder);

            List<string> animsPath = new List<string>(foundAnimations);
            animsPath.RemoveAll(x => { return x.Contains(ConstsFolders.ART_EXPORT_ROOT) || x.StartsWith(PREVIEW); });

            string placeSearchPath = string.Empty;

            string containerPath = $"{Path.GetFileNameWithoutExtension(assetPath).Replace($"_{ConstsEditor.ANIMATIONS}", string.Empty)}.{ConstsEditor.ASSET}";

            string placementAssetPath = assetPath.Replace(Path.GetFileName(assetPath), containerPath);
            FileInfo libraryFileInfo = GetFileInfoForPass(FbxExporterEditorWindow.PASS_ANIM_LIBRARY, placementAssetPath, originalPath, ref placeSearchPath, FileInfoMode.MainUse);

            DrawTitle("Animations data available:", "Generate ALL", () =>
            {
                List<string> clips = new List<string>();
                DrawOrGenerateAllAnimations(false, assetInfo, assetPath, originalPath, animsPath, clips, statusBoxOption);

                AnimationBuilderHelper.GenerateAnimationLibrary(placeSearchPath, clips, NonAdditiveId);
            });

            List<string> createdClips = new List<string>();

            DrawTestAndGenerate<ScriptableObject>(false, assetInfo, libraryFileInfo, FbxExporterEditorWindow.PASS_ANIM_LIBRARY, placeSearchPath, statusBoxOption, pass =>
            {
                AnimationBuilderHelper.GenerateAnimationLibrary(placeSearchPath, createdClips, nonAdditiveId);
            });

            DrawOrGenerateAllAnimations(true, assetInfo, assetPath, originalPath, animsPath, createdClips, statusBoxOption);
        }

        private void DrawOrGenerateAllAnimations(bool doDraw, FileInfo sourceInfo, string assetPath, string originalPath, List<string> sourceAssets, List<string> clips, GUILayoutOption statusBoxOption)
        {
            bool doSpace = true;
            foreach (string sourceAsset in sourceAssets)
            {
                sourceInfo = new FileInfo(sourceAsset);
                string foundClip = AnimationBuilderHelper.GetAnimationName(Path.GetFileName(sourceAsset)); // as AnimationClip;
                string exportPath = string.Empty;
                assetPath = assetPath.Replace(Path.GetFileName(assetPath), $"{foundClip}.{ConstsEditor.ANIM}");
                FileInfo exportInfo = GetFileInfoForPass(FbxExporterEditorWindow.PASS_ANIMATIONS, assetPath, originalPath, ref exportPath, FileInfoMode.MainUse);

                clips.Add(exportPath);

                if (doDraw)
                {
                    DrawTestAndGenerate<AnimationClip>(doSpace, sourceInfo, exportInfo, FbxExporterEditorWindow.PASS_ANIMATIONS, exportPath, statusBoxOption, pass =>
                    {
                        AnimationBuilderHelper.GenerateCleanedUpAnimation(sourceAsset, exportPath);
                    });

                    doSpace = false;
                }
                else
                {
                    AnimationBuilderHelper.GenerateCleanedUpAnimation(sourceAsset, exportPath);
                }
            }
        }

        private void DrawMainAsset(FileInfo assetInfo, List<CategoryContentType> lodRequired, string assetPath, string originalPath, GUILayoutOption statusBoxOption, Action generateAll, Action<int> generateThis)
        {
            DrawTitle("Required exported valid datas", "Generate ALL dependencies", generateAll);

            for (int pass = 0; pass < lodRequired.Count + PADDING; pass++)
            {
                string searchPath = string.Empty;
                FileInfo fileInfo = GetFileInfoForPass(pass, assetPath, originalPath, ref searchPath, FileInfoMode.MainUse);
                if (!fileInfo.Exists)
                {
                    if (pass >= FbxExporterEditorWindow.PASS_LOD && lodRequired[pass - PADDING].HasBoth(CategoryContentType.EXTRA))
                    {
                        continue;
                    }
                }

                DrawTestAndGenerate<GameObject>(pass == 1, assetInfo, fileInfo, pass, searchPath, statusBoxOption, generateThis);
            }
        }

        private void DrawLines()
        {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(false))
            {
                using (new EditorGUI.IndentLevelScope(ConstsEditor.INDENT_BACKUP))
                {
                    GUI.enabled = true;

                    DrawnLine lastLine = null;
                    foreach (DrawnLine drawnLine in drawnLines)
                    {
                        if (drawnLine.doSpace)
                        {
                            EditorGUILayout.Space();
                        }

                        if (drawnLine is DrawnTitle title)
                        {
                            if (lastLine != null)
                            {
                                EditorGUI.indentLevel--;
                                EditorGUILayout.Space();
                            }

                            DoDrawTitle(title.title, title.buttonTitle, title.generateAll);
                            EditorGUI.indentLevel++;
                        }
                        else if (drawnLine is DrawnTestGenerator test)
                        {
                            if (lastLine is DrawnTitle)
                            {
                                EditorGUILayout.Space();
                            }

                            DoDrawTestAndGenerate(test.sourceInfo, test.targetInfo, test.pass, test.searchPath, test.statusBoxOption, test.generateThis, test.loaderFunc);
                        }

                        lastLine = drawnLine;
                    }
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTitle(string title, string buttonTitle, Action generateAll)
        {
            drawnLines.Add(new DrawnTitle
            {
                title = title,
                buttonTitle = buttonTitle,
                generateAll = generateAll
            });
        }

        private void DrawTestAndGenerate<T>(bool doSpace, FileInfo sourceInfo, FileInfo targetInfo, int pass, string searchPath, GUILayoutOption statusBoxOption, Action<int> generateThis)
            where T : UnityEngine.Object
        {
            drawnLines.Add(new DrawnTestGenerator
            {
                doSpace = doSpace,
                sourceInfo = sourceInfo,
                targetInfo = targetInfo,
                pass = pass,
                searchPath = searchPath,
                statusBoxOption = statusBoxOption,
                generateThis = generateThis,
                loaderFunc = () =>
                {
                    return AssetDatabase.LoadAssetAtPath<T>(searchPath);
                }
            });
        }

        private void DoDrawTitle(string title, string buttonTitle, Action generateAll)
        {
            Rect labelRect = EditorGUILayout.GetControlRect();
            DrawButton(ref labelRect, buttonTitle, generateAll);

            EditorGUI.LabelField(labelRect, title);
        }

        private void DoDrawTestAndGenerate(FileInfo sourceInfo, FileInfo targetInfo, int pass, string searchPath, GUILayoutOption statusBoxOption, Action<int> generateThis, Func<UnityEngine.Object> loaderFunc)
        {
            int r = 0;
            Rect rect = EditorGUILayout.GetControlRect();
            Rect[] rects = targetInfo.Exists
                ? RectHelper.SplitX(ref rect, Split.FixedSize(100), Split.FixedSize(100), 100, Split.FixedSize(100))
                : RectHelper.SplitX(ref rect, Split.FixedSize(100), 100, Split.FixedSize(100));

            if (!targetInfo.Exists)
            {
                GUIHelper.ShowStatusBox(rects[r++], Color.red, ConstsEditor.MISSING, statusBoxOption);
                EditorGUI.LabelField(rects[r++], $"EXPECTED: {searchPath}");
            }
            else
            {
                UnityEngine.Object foundObject = loaderFunc();

                if (sourceInfo.LastWriteTime > targetInfo.LastWriteTime)
                {
                    GUIHelper.ShowStatusBox(rects[r++], Color.yellow, ConstsEditor.OUT_OF_DATE, statusBoxOption);
                }
                else
                {
                    GUIHelper.ShowStatusBox(rects[r++], Color.green, ConstsEditor.FOUND, statusBoxOption);
                }

                GUIHelper.AllowFocus(rects[r++], foundObject);

                string title = GetTitleForPass(pass);
                EditorGUI.LabelField(rects[r++], $"{title}: {targetInfo.Name}");
            }

            switch (pass)
            {
                case FbxExporterEditorWindow.PASS_REFERENCE:
                case FbxExporterEditorWindow.PASS_UNIQUE:
                case FbxExporterEditorWindow.PASS_LOD:
                case FbxExporterEditorWindow.PASS_ANIMATIONS:
                case FbxExporterEditorWindow.PASS_ANIM_LIBRARY:
                {
                    Rect buttonRect = rects[r++];
                    DrawButton(ref buttonRect, "Generate THIS", () => { generateThis.Invoke(pass); });
                    break;
                }
            }
        }

        private FileInfo GetFileInfoForPass(int pass, string assetPath, FileInfoMode mode)
        {
            string searchPath = string.Empty;
            return GetFileInfoForPass(pass, assetPath, ref searchPath, mode);
        }

        private FileInfo GetFileInfoForPass(int pass, string assetPath, ref string searchPath, FileInfoMode mode)
        {
            string originalPath = string.Empty;
            return GetFileInfoForPass(pass, assetPath, originalPath, ref searchPath, mode);
        }

        private FileInfo GetFileInfoForPass(int pass, string assetPath, string originalPath, ref string searchPath, FileInfoMode mode)
        {
            switch (pass)
            {
                case FbxExporterEditorWindow.PASS_INGAME:
                case FbxExporterEditorWindow.PASS_REFERENCE:
                {
                    //I know this is weird but switch in c# forces me to do that.....
                    if (pass == 0 && mode == FileInfoMode.MainUse)
                    {
                        string artReplace = string.Format("/{0}/", ConstsFolders.ART_ROOT);
                        string prefabReplace = string.Format("/{0}/", ConstsFolders.PREFABS);
                        searchPath = string.IsNullOrEmpty(originalPath) ? assetPath : originalPath;
                        searchPath = searchPath.Replace(artReplace, prefabReplace);
                        string extension = Path.GetExtension(searchPath);
                        searchPath = searchPath.Replace(extension, $".{ConstsEditor.PREFAB}");
                        return new FileInfo(searchPath);
                    }

                    if (mode == FileInfoMode.RegenerateButton)
                    {
                        return GetFileInfo(assetPath.Replace(Path.GetExtension(assetPath), $".{ConstsEditor.PREFAB}"));
                    }
                    else
                    {
                        return GetFileInfo(assetPath.Replace(Path.GetExtension(assetPath), $".{ConstsEditor.PREFAB}"), ref searchPath);
                    }
                }
                case FbxExporterEditorWindow.PASS_UNIQUE:
                {
                    if (mode == FileInfoMode.RegenerateButton)
                    {
                        return GetRelatedFileInfo(assetPath, ConstsEditor.UNIQUE);
                    }
                    else
                    {
                        return GetRelatedFileInfo(assetPath, ConstsEditor.UNIQUE, ref searchPath);
                    }
                }
                case FbxExporterEditorWindow.PASS_ANIMATIONS:
                {
                    return GetRelatedFileInfoAnim(assetPath, ref searchPath);
                }
                case FbxExporterEditorWindow.PASS_ANIM_LIBRARY:
                {
                    return GetFileInfo(assetPath, ref searchPath);
                }
                default:
                {
                    if (mode == FileInfoMode.RegenerateButton)
                    {
                        return GetFileInfoForLOD(assetPath, pass - PADDING);
                    }
                    else
                    {
                        return GetFileInfoForLOD(assetPath, pass - PADDING, ref searchPath);
                    }
                }
            }
        }

        private string GetTitleForPass(int pass)
        {
            switch (pass)
            {
                case FbxExporterEditorWindow.PASS_INGAME:
                {
                    return "Ingame prefab     ";
                }
                case FbxExporterEditorWindow.PASS_REFERENCE:
                {
                    return "Reference prefab  ";
                }
                case FbxExporterEditorWindow.PASS_ANIM_LIBRARY:
                {
                    return "Animation Library ";
                }
                case FbxExporterEditorWindow.PASS_ANIMATIONS:
                {
                    return "Animation         ";
                }
                default:
                {
                    return "Dependencies      ";
                }
            }
        }

        private void DrawButton(string text, Action action)
        {
            if (GUILayout.Button(text, GUILayout.Width(GENERATE_BUTTON_SIZE)))
            {
                action();
            }
        }

        private void DrawButton(ref Rect foldoutRect, string text, Action action)
        {
            Rect[] splits = RectHelper.SplitX(ref foldoutRect, 1, Split.FixedSize(GENERATE_BUTTON_SIZE));
            foldoutRect = splits[0];
            if (GUI.Button(splits[1], text))
            {
                action();
            }
        }

        private FileInfo GetFileInfoForLOD(string assetPath, int lodIndex)
        {
            string lodPath = string.Empty;
            return GetFileInfoForLOD(assetPath, lodIndex, ref lodPath);
        }

        private FileInfo GetFileInfoForLOD(string assetPath, int lodIndex, ref string lodPath)
        {
            return GetRelatedFileInfo(assetPath, $"{ConstsEditor.LOD}{lodIndex}", ref lodPath);
        }

        private FileInfo GetRelatedFileInfo(string assetPath, string suffix)
        {
            string resultPath = string.Empty;
            return GetRelatedFileInfo(assetPath, suffix, ref resultPath);
        }

        private FileInfo GetRelatedFileInfo(string assetPath, string suffix, ref string resultPath)
        {
            resultPath = ExporterHelper.GetExportAssetPath(assetPath, $"{Path.GetFileNameWithoutExtension(assetPath)}_{suffix}");
            return string.IsNullOrEmpty(resultPath) ? new FileInfo("INVALID") : new FileInfo(resultPath);
        }

        private FileInfo GetRelatedFileInfoAnim(string assetPath, ref string resultPath)
        {
            assetPath = $"{AnimationBuilderHelper.GetAnimationFolder(Path.GetDirectoryName(assetPath))}{Path.AltDirectorySeparatorChar}{Path.GetFileName(assetPath)}";
            return GetFileInfo(assetPath, ref resultPath);
        }

        private FileInfo GetFileInfo(string assetPath)
        {
            string resultPath = string.Empty;
            return GetFileInfo(assetPath, ref resultPath);
        }

        private FileInfo GetFileInfo(string assetPath, ref string resultPath)
        {
            resultPath = ExporterHelper.GetExportAssetPath(assetPath, Path.GetFileNameWithoutExtension(assetPath));
            return string.IsNullOrEmpty(resultPath) ? new FileInfo("INVALID") : new FileInfo(resultPath);
        }
        #endregion

        #region Nested type: DrawnLine
        private class DrawnLine
        {
            #region Fields
            public bool doSpace;
            #endregion
        }
        #endregion

        #region Nested type: DrawnTestGenerator
        private class DrawnTestGenerator : DrawnLine
        {
            #region Fields
            public FileInfo sourceInfo;
            public FileInfo targetInfo;
            public int pass;
            public string searchPath;
            public GUILayoutOption statusBoxOption;
            public Action<int> generateThis;
            public Func<UnityEngine.Object> loaderFunc;
            #endregion
        }
        #endregion

        #region Nested type: DrawnTitle
        private class DrawnTitle : DrawnLine
        {
            #region Fields
            public string title;
            public string buttonTitle;
            public Action generateAll;
            #endregion
        }
        #endregion
    }
}

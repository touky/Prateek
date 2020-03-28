namespace Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder
{
    using System.Collections.Generic;
    using Mayfair.Core.Code.GUIExt;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Code.Utils.Helpers;
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.Enums;
    using Mayfair.Core.Editor.EditorWindows.ShaderUsageFinder.ShaderFinderParameter;
    using UnityEditor;
    using UnityEditor.VersionControl;
    using UnityEngine;

    public class ShaderUsageFinderEditorWindow : BaseEditorWindow
    {
        #region Static and Constants
        private const string WINDOW_TITLE = "Material Finder";
        #endregion

        #region Fields
        private bool includePartialValid;
        private SortBy activeSortBy = SortBy.Nothing;
        private Shader searchShader;
        private string searchShaderName = string.Empty;
        private string searchMaterialName = string.Empty;
        private List<SearchResult> searchResults = new List<SearchResult>();
        private Vector2 scroll;

        private bool instancingSetAllStatus = true;

        private GUIStyle titleBG;
        private GUIStyle titleSort;
        private GUIStyle titleText;
        private GUIStyle mainBG;
        private GUIStyle[] buttonStyle;

        private ShaderFinderParameter.ShaderFinderParameter[] parameters;
        private InstancingSetAllParameter instancingSetAll;
        #endregion

        #region Unity Methods
        protected override void OnEnable()
        {
            base.OnEnable();

            mainBG = GUIStyleHelper.CreateStyle("mainBG", Color.black, ColorHelper.editor.background, Color.grey);
            titleBG = GUIStyleHelper.CreateStyle("titleBG", Color.black, ColorHelper.editor.backgroundDark, Color.grey);
            titleBG.alignment = TextAnchor.MiddleCenter;
            titleSort = GUIStyleHelper.CreateStyle("titleSort", Color.black, ColorHelper.editor.backgroundDark, Color.white);
            titleSort.alignment = TextAnchor.MiddleCenter;

            buttonStyle = new[]
            {
                GUIStyleHelper.CreateStyle("buttonStyle[0]", Color.red, ColorHelper.red.dark25, Color.red),
                GUIStyleHelper.CreateStyle("buttonStyle[1]", Color.green, ColorHelper.green.dark25, Color.green),
                GUIStyleHelper.CreateStyle("buttonStyle[2]", Color.grey, ColorHelper.grey.dark50, Color.grey)
            };

            parameters = new ShaderFinderParameter.ShaderFinderParameter[]
            {
                new SelectParameter(new GUIContent("Show")),
                new ResultNameParameter(ResultNameLabelType.ShaderName),
                new InstancingParameter("Instancing", buttonStyle),
                new PropertyParameter("Alpha cut", buttonStyle, new[] {"_Cutoff", "_CutOff"}, new[] {"_ALPHATEST_ON"}),
                new TextureParameter("Main texture", buttonStyle, new[] {"_MainTex"}),
                new ResultNameParameter(ResultNameLabelType.MaterialName),
                new ResultNameParameter(ResultNameLabelType.Location)
            };

            instancingSetAll = new InstancingSetAllParameter(buttonStyle);
        }
        #endregion

        #region Unity EditorOnly Methods
        protected override void OnGUI()
        {
            if (titleText == null)
            {
                titleText = GUIStyleHelper.CreateStyle("titleText", GUI.skin.label);
                titleText.alignment = TextAnchor.UpperCenter;
            }

            float lineHeight = Mathf.Max(24, GUIStyle.none.lineHeight);

            SearchValidation searchValidation = SearchValidation.Nothing;
            int validationCount = 0;

            BuildSearchValidation(ref searchValidation, ref validationCount);

            EditorGUILayout.Space();
            DrawSearchBar(searchValidation, validationCount);

            float[] columnsSplit = new float[parameters.Length];
            AdjustColumnSizes(columnsSplit);

            EditorGUILayout.Space();
            EditorGUILayout.Separator();

            DrawTitle(columnsSplit, lineHeight);

            DrawParameters(columnsSplit, lineHeight, searchValidation);
        }
        #endregion

        #region Class Methods
        private void BuildSearchValidation(ref SearchValidation searchValidation, ref int validationCount)
        {
            if (searchMaterialName != string.Empty)
            {
                validationCount++;
                searchValidation |= SearchValidation.MaterialName;
            }

            if (searchShaderName != string.Empty)
            {
                validationCount++;
                searchValidation |= SearchValidation.ShaderName;
            }

            if (searchShader != null)
            {
                validationCount++;
                searchValidation |= SearchValidation.ShaderRef;
            }
        }

        private void DrawSearchBar(SearchValidation searchValidation, int validationCount)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                bool doSearch = false;
                float searchWidth = 160f;
                using (new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(searchWidth)))
                {
                    doSearch = GUI.Button(EditorGUILayout.GetControlRect(GUILayout.MaxWidth(searchWidth)), "Search Materials");
                    includePartialValid = EditorGUILayout.ToggleLeft("Include partialy valid", includePartialValid, GUILayout.MaxWidth(searchWidth));
                }

                float byWidth = 120f;
                using (new EditorGUILayout.VerticalScope(GUILayout.MaxWidth(byWidth)))
                {
                    EditorGUILayout.LabelField("By Material name: ", GUILayout.MaxWidth(byWidth));
                    EditorGUILayout.LabelField("By Shader name: ", GUILayout.MaxWidth(byWidth));
                    EditorGUILayout.LabelField("By Shader: ", GUILayout.MaxWidth(byWidth));
                }

                using (new EditorGUILayout.VerticalScope())
                {
                    Shader prev = searchShader;
                    searchMaterialName = EditorGUILayout.TextField(searchMaterialName);
                    searchShaderName = EditorGUILayout.TextField(searchShaderName);
                    searchShader = EditorGUILayout.ObjectField(searchShader, typeof(Shader), false) as Shader;
                    if (doSearch)
                    {
                        string shaderPath = AssetDatabase.GetAssetPath(searchShader);
                        string[] allMaterials = AssetDatabase.FindAssets("t:Material");
                        searchResults.Clear();

                        for (int i = 0; i < allMaterials.Length; i++)
                        {
                            allMaterials[i] = AssetDatabase.GUIDToAssetPath(allMaterials[i]);
                            Material material = AssetDatabase.LoadAssetAtPath(allMaterials[i], typeof(Material)) as Material;
                            if (material != null)
                            {
                                bool doAdd = false;

                                bool matNameIsValid = material.name.ToLower().Contains(searchMaterialName.ToLower());
                                bool shaNameIsValid = material.shader.name.ToLower().Contains(searchShaderName.ToLower());
                                bool shaRefIsValid = material.shader == searchShader;

                                SearchValidation foundValidation = SearchValidation.Nothing;
                                if (searchValidation.Has(SearchValidation.MaterialName) && matNameIsValid)
                                {
                                    foundValidation |= SearchValidation.MaterialName;
                                }

                                if (searchValidation.Has(SearchValidation.ShaderName) && shaNameIsValid)
                                {
                                    foundValidation |= SearchValidation.ShaderName;
                                }

                                if (searchValidation.Has(SearchValidation.ShaderRef) && shaRefIsValid)
                                {
                                    foundValidation |= SearchValidation.ShaderRef;
                                }

                                doAdd = validationCount == 1
                                    ? foundValidation.Has(SearchValidation.MaterialName | SearchValidation.ShaderName | SearchValidation.ShaderRef)
                                    : (!searchValidation.Has(SearchValidation.MaterialName) || foundValidation.Has(SearchValidation.MaterialName))
                                   && (!searchValidation.Has(SearchValidation.ShaderName) || foundValidation.Has(SearchValidation.ShaderName))
                                   && (!searchValidation.Has(SearchValidation.ShaderRef) || foundValidation.Has(SearchValidation.ShaderRef));

                                if (includePartialValid && searchValidation.Has(foundValidation))
                                {
                                    doAdd = true;
                                }

                                if (doAdd)
                                {
                                    searchResults.Add(new SearchResult
                                    {
                                        validation = foundValidation,
                                        instance = material,
                                        location = allMaterials[i],
                                        material = allMaterials[i].Substring(allMaterials[i].LastIndexOf("/") + 1).Replace(".mat", ""),
                                        shader = material.shader.name
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }

        private void AdjustColumnSizes(float[] columnsSplit)
        {
            for (int c = 0; c < columnsSplit.Length; c++)
            {
                ShaderFinderParameter.ShaderFinderParameter parameter = parameters[c];
                columnsSplit[c] = parameter.GetTitleWidth(titleText);

                LabelParameter labelParam = parameter as LabelParameter;
                if (labelParam != null)
                {
                    //Gather max sizes from datas
                    for (int i = 0; i < searchResults.Count; i++)
                    {
                        SearchResult result = searchResults[i];
                        columnsSplit[c] = Mathf.Max(columnsSplit[c], titleText.CalcSize(new GUIContent(labelParam.GetLabel(result))).x);
                    }
                }
            }
        }

        private void DrawTitle(float[] columnsSplit, float lineHeight)
        {
            SortBy requestSortBy = SortBy.Nothing;
            float titleHeight = lineHeight * 2;
            Rect bgRect = EditorGUILayout.GetControlRect(GUILayout.Height(titleHeight));

            Rect titleRect = RectHelper.TruncateY(ref bgRect, titleHeight);
            titleRect = RectHelper.Inflate(titleRect, -1, -1);

            //Titles
            {
                bool doInstancingSetAll = false;
                Rect[] titleRects = RectHelper.SplitX(ref titleRect, columnsSplit);

                for (int t = 1; t < titleRects.Length; t++)
                {
                    ShaderFinderParameter.ShaderFinderParameter parameter = parameters[t];

                    requestSortBy |= parameter.OnTitleGUI(titleRects[t], parameter.Sort.Has(activeSortBy) ? titleSort : titleBG, titleText);

                    if (parameter is InstancingParameter)
                    {
                        Rect rInstAll = titleRects[t];
                        Rect rInst = RectHelper.TruncateY(ref rInstAll, rInstAll.height / 2);
                        if (GUI.Button(RectHelper.Inflate(rInstAll, -2), instancingSetAllStatus ? "Set all ON" : "Set all OFF", instancingSetAllStatus ? buttonStyle[1] : buttonStyle[0]))
                        {
                            doInstancingSetAll = true;
                        }
                    }
                }

                if (requestSortBy != SortBy.Nothing)
                {
                    if (activeSortBy == requestSortBy)
                    {
                        activeSortBy = requestSortBy;
                        if (activeSortBy.Has(SortBy.Inverted))
                        {
                            activeSortBy &= ~SortBy.Inverted;
                        }
                        else
                        {
                            activeSortBy |= SortBy.Inverted;
                        }
                    }
                    else
                    {
                        activeSortBy = requestSortBy;
                    }

                    searchResults.Sort((a, b) =>
                    {
                        string aName = string.Empty;
                        string bName = string.Empty;
                        switch (requestSortBy)
                        {
                            case SortBy.Shader:
                            {
                                aName = a.shader;
                                bName = b.shader;
                                break;
                            }
                            case SortBy.Material:
                            {
                                aName = a.material;
                                bName = b.material;
                                break;
                            }
                            case SortBy.Location:
                            {
                                aName = a.location;
                                bName = b.location;
                                break;
                            }
                        }

                        if (activeSortBy.Has(SortBy.Inverted))
                        {
                            return string.Compare(bName, aName);
                        }

                        return string.Compare(aName, bName);
                    });
                }

                if (doInstancingSetAll)
                {
                    if (instancingSetAll != null)
                    {
                        instancingSetAll.Set(instancingSetAllStatus);
                        for (int r = 0; r < searchResults.Count; r++)
                        {
                            SearchResult result = searchResults[r];

                            instancingSetAll.DoAction(EditorAction.DoAction | EditorAction.Checkout | EditorAction.RecordUndo | EditorAction.SetDirty, result);

                            Provider.Checkout(result.instance, CheckoutMode.Asset);
                            EditorUtility.SetDirty(result.instance);
                        }

                        instancingSetAllStatus = !instancingSetAllStatus;
                    }
                }
            }
        }

        private void DrawParameters(float[] columnsSplit, float lineHeight, SearchValidation searchValidation)
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);
            {
                if (searchResults.Count > 0)
                {
                    Rect bgRect = EditorGUILayout.GetControlRect(GUILayout.Height(searchResults.Count * lineHeight));
                    GUI.Box(bgRect, GUIContent.none, mainBG);

                    Rect[] resultRects = RectHelper.SplitX(ref bgRect, columnsSplit);
                    for (int r = 0; r < resultRects.Length; r++)
                    {
                        Rect resultRect = resultRects[r];
                        resultRect = RectHelper.Inflate(resultRect, -1, -1);
                        GUI.Box(resultRect, GUIContent.none, mainBG);

                        Rect[] searchRects = RectHelper.SplitY(ref resultRect, searchResults.Count);
                        for (int s = 0; s < searchRects.Length; s++)
                        {
                            SearchResult result = searchResults[s];
                            using (new EditorGUI.DisabledGroupScope(searchValidation != result.validation))
                            {
                                Rect searchRect = searchRects[s];
                                searchRect = RectHelper.Inflate(searchRect, -2, -2);

                                EditorAction action = parameters[r].OnContentGUI(searchRect, result);
                                if (action != EditorAction.Nothing)
                                {
                                    parameters[r].DoAction(action, result);
                                }
                            }
                        }
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP1_1)]
        public static void Open()
        {
            DoOpen<ShaderUsageFinderEditorWindow>(WINDOW_TITLE);
        }
        #endregion
    }
}

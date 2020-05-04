namespace Mayfair.Core.Editor.BuildFlags
{
    using System;
    using System.Collections.Generic;
    using Mayfair.Core.Code.MathExt;
    using Mayfair.Core.Editor.BuildFlags.Enums;
    using Mayfair.Core.Editor.EditorWindows;
    using Mayfair.Core.Editor.GUI;
    using UnityEditor;
    using UnityEngine;

#if UNITY_EDITOR

#endif

    public class BuildFlagsEditorWindow : BaseEditorWindow
    {
        #region Static and Constants
        private const string WINDOW_TITLE = "Build flag Editor";
        #endregion

        #region Fields
        private BuildFlagsConfiguration configuration = new BuildFlagsConfiguration();
        private char[] SYMBOLS_SEPARATOR = {';'};

        private BuildFlagsStatus status = BuildFlagsStatus.Nothing;
        private float[] guiColumns = new float[1 + (int) BuildFlagsConfiguration.BuildSymbolsType.MAX];

        private TargetSymbols[] targetSymbols =
        {
            new TargetSymbols {@group = BuildTargetGroup.Standalone, target = BuildTarget.StandaloneWindows},
            new TargetSymbols {@group = BuildTargetGroup.Android, target = BuildTarget.Android},
            new TargetSymbols {@group = BuildTargetGroup.iOS, target = BuildTarget.iOS}
        };
        #endregion

        #region Class Methods
        [MenuItem(EditorMenuOrderHelper.WINDOW_MENU + WINDOW_TITLE, priority = EditorMenuOrderHelper.BAR_MENU_GROUP1_0)]
        public static void Open()
        {
            DoOpen<BuildFlagsEditorWindow>(WINDOW_TITLE);
        }

        private void InitSymbols()
        {
            for (int t = 0; t < this.targetSymbols.Length; t++)
            {
                TargetSymbols target = this.targetSymbols[t];
                string symbolsText = PlayerSettings.GetScriptingDefineSymbolsForGroup(target.group);
                string[] symbols = symbolsText.Split(this.SYMBOLS_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);
                if (symbols.Length == 0)
                {
                    continue;
                }

                target.symbols = new List<BuildFlagsConfiguration.BuildSymbolsType>();
                for (int s = 0; s < symbols.Length; s++)
                {
                    BuildFlagsConfiguration.BuildSymbolsType value = BuildFlagsConfiguration.BuildSymbolsType.NOTHING;
                    string symbol = symbols[s];
                    if (Enum.TryParse(symbol, out value))
                    {
                        target.symbols.Add(value);
                    }
                    else
                    {
                        target.saved = symbol + this.SYMBOLS_SEPARATOR[0];
                    }
                }

                this.targetSymbols[t] = target;
            }
        }

        private void ApplyBuildSymbols()
        {
            this.status |= BuildFlagsStatus.PendingSave;

            for (int t = 0; t < this.targetSymbols.Length; t++)
            {
                TargetSymbols targetSymbol = this.targetSymbols[t];
                string symbols = targetSymbol.saved;

                if (targetSymbol.symbols != null)
                {
                    for (int s = 0; s < targetSymbol.symbols.Count; s++)
                    {
                        symbols += targetSymbol.symbols[s].ToString() + this.SYMBOLS_SEPARATOR[0];
                    }
                }

                PlayerSettings.SetScriptingDefineSymbolsForGroup(targetSymbol.group, symbols);
            }
        }
        #endregion

        #region Unity defaults
        protected override void OnEnable()
        {
            base.OnEnable();

            InitSymbols();

            this.guiColumns = new float[1 + this.targetSymbols.Length];
            for (int c = 0; c < this.guiColumns.Length; c++)
            {
                this.guiColumns[c] = 10;
                if (c == 0)
                {
                    this.guiColumns[c] = -160;
                }
            }
        }

        protected void OnInspectorUpdate()
        {
            if (this.status.HasEither(BuildFlagsStatus.ApplySymbols))
            {
                ApplyBuildSymbols();

                this.status &= ~(BuildFlagsStatus.ApplySymbols | BuildFlagsStatus.NeedSymbolUpdate);
            }

            if (this.status.HasEither(BuildFlagsStatus.PendingSave))
            {
                this.status |= BuildFlagsStatus.NeedRepaint;
                if (EditorApplication.isCompiling)
                {
                    this.status &= ~BuildFlagsStatus.PendingSave;
                    this.status |= BuildFlagsStatus.NeedSettingsSave;
                }
            }
            else if (this.status.HasEither(BuildFlagsStatus.NeedSettingsSave))
            {
                this.status |= BuildFlagsStatus.NeedRepaint;
                if (!EditorApplication.isCompiling)
                {
                    this.status &= ~BuildFlagsStatus.NeedSettingsSave;
                    AssetDatabase.SaveAssets();
                }
            }

            if (this.status.HasEither(BuildFlagsStatus.NeedRepaint))
            {
                this.status &= ~BuildFlagsStatus.NeedRepaint;

                Repaint();
            }
        }

        protected override void OnGUI()
        {
            string[] thisPath = AssetDatabase.FindAssets(typeof(BuildFlagsConfiguration).Name);
            if (thisPath != null && thisPath.Length > 0)
            {
                UnityEngine.Object obj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(thisPath[0]), typeof(MonoScript));
                EditorGUILayout.ObjectField("Change keywords in: ", obj, typeof(MonoScript), false);
                EditorGUILayout.Space();
            }

            Rect targetRect = EditorGUILayout.GetControlRect(GUILayout.Height(20));
            Rect[] targetRects = RectHelper.SplitX(ref targetRect, this.guiColumns);
            for (int r = 1; r < targetRects.Length; r++)
            {
                Rect rect = targetRects[r];
                TargetSymbols targetSymbol = this.targetSymbols[r - 1];

                EditorGUI.LabelField(rect, targetSymbol.group.ToString());
            }

            for (int s = 0; s < (int) BuildFlagsConfiguration.BuildSymbolsType.MAX; s++)
            {
                BuildFlagsConfiguration.BuildSymbolsType symbol = (BuildFlagsConfiguration.BuildSymbolsType) s;
                if (symbol == BuildFlagsConfiguration.BuildSymbolsType.NOTHING)
                {
                    continue;
                }

                BuildFlagsConfiguration.BuildSymbolsType prevSymbol = (BuildFlagsConfiguration.BuildSymbolsType) (s - 1);

                SymbolRelationship relationship = new SymbolRelationship();
                SymbolRelationship prevRelationship = new SymbolRelationship();
                for (int r = 0; r < this.configuration.relationships.Length; r++)
                {
                    if (this.configuration.relationships[r].symbol == symbol)
                    {
                        relationship = this.configuration.relationships[r];
                    }
                    else if (this.configuration.relationships[r].symbol == prevSymbol)
                    {
                        prevRelationship = this.configuration.relationships[r];
                    }
                }

                if (relationship.exclude != null && !relationship.exclude.Contains(prevSymbol)
                 || prevRelationship.exclude != null && !prevRelationship.exclude.Contains(symbol))
                {
                    EditorGUILayout.Space();
                }

                Rect symbolRect = EditorGUILayout.GetControlRect();
                Rect[] symbolRects = RectHelper.SplitX(ref symbolRect, this.guiColumns);
                for (int r = 0; r < symbolRects.Length; r++)
                {
                    Rect rect = symbolRects[r];
                    if (r == 0)
                    {
                        EditorGUI.LabelField(rect, symbol.ToString());
                        continue;
                    }

                    TargetSymbols targetSymbol = this.targetSymbols[r - 1];
                    bool hasSymbol = false;
                    if (targetSymbol.symbols != null)
                    {
                        for (int t = 0; t < targetSymbol.symbols.Count; t++)
                        {
                            if (targetSymbol.symbols[t] != symbol)
                            {
                                continue;
                            }

                            hasSymbol = true;
                            break;
                        }
                    }

                    Rect boxRect = RectHelper.Inflate(rect, -10, -1);
                    GUIHelper.ShowStatusBox(boxRect, hasSymbol, string.Empty, "ON", "OFF");

                    Event e = Event.current;
                    if (e.type == EventType.MouseUp && boxRect.Contains(e.mousePosition))
                    {
                        if (targetSymbol.symbols == null)
                        {
                            targetSymbol.symbols = new List<BuildFlagsConfiguration.BuildSymbolsType>();
                        }

                        if (hasSymbol)
                        {
                            targetSymbol.symbols.Remove(symbol);
                        }
                        else
                        {
                            targetSymbol.symbols.Add(symbol);

                            for (int rs = 0; relationship.exclude != null && rs < relationship.exclude.Count; rs++)
                            {
                                targetSymbol.symbols.Remove(relationship.exclude[rs]);
                            }

                            for (int rs = 0; relationship.include != null && rs < relationship.include.Count; rs++)
                            {
                                if (!targetSymbol.symbols.Contains(relationship.include[rs]))
                                {
                                    targetSymbol.symbols.Add(relationship.include[rs]);
                                }
                            }
                        }

                        this.status |= BuildFlagsStatus.NeedSymbolUpdate;
                        this.status |= BuildFlagsStatus.NeedRepaint;
                        Repaint();
                    }

                    this.targetSymbols[r - 1] = targetSymbol;
                }
            }

            EditorGUILayout.Space();

            using (new EditorGUI.DisabledGroupScope(!this.status.HasEither(BuildFlagsStatus.NeedSymbolUpdate)))
            {
                Rect rect = EditorGUILayout.GetControlRect();
                RectHelper.TruncateX(ref rect, rect.width * (3f / 4f));

                if (GUI.Button(rect, "Apply build flags"))
                {
                    this.status |= BuildFlagsStatus.ApplySymbols;
                }
            }

            if (this.status.HasEither(BuildFlagsStatus.NeedRepaint))
            {
                this.status &= ~BuildFlagsStatus.NeedRepaint;

                Repaint();
            }
        }
        #endregion Unity defaults

        #region Nested type: SymbolRelationship
        public struct SymbolRelationship
        {
            public BuildFlagsConfiguration.BuildSymbolsType symbol;
            public List<BuildFlagsConfiguration.BuildSymbolsType> include;
            public List<BuildFlagsConfiguration.BuildSymbolsType> exclude;
        }
        #endregion

        #region Nested type: TargetSymbols
        public struct TargetSymbols
        {
            public BuildTarget target;
            public BuildTargetGroup group;
            public List<BuildFlagsConfiguration.BuildSymbolsType> symbols;
            public string saved;
        }
        #endregion
    }
}

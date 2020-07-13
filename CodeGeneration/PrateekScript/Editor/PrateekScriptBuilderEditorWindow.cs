namespace Prateek.CodeGeneration.PrateekScript.Editor
{
    using System.Collections.Generic;
    using Prateek.CodeGeneration.BackendTools.Editor;
    using Prateek.CodeGeneration.Code.PrateekScript;
    using Prateek.CodeGeneration.Code.PrateekScript.ScriptAnalysis.Utils;
    using Prateek.CodeGeneration.PrateekScript.Editor.ScriptActions;
    using UnityEditor;
    using UnityEngine.UIElements;

    internal static class EditorExtension
    {
        public static List<KeywordUsage> GetKeywords(this Dictionary<string, List<KeywordUsage>> source, string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return null;
            }

            if (!source.ContainsKey(keyword))
            {
                return null;
            }

            return source[keyword];
        }

        public static void SafeAdd(this Dictionary<string, List<KeywordUsage>> source, string key, KeywordUsage value)
        {
            if (!source.ContainsKey(key))
            {
                source.Add(key, new List<KeywordUsage>());
            }

            var list = source[key];
            if (!list.Contains(value))
            {
                list.Add(value);
            }
        }
    }

    public class PrateekScriptBuilderEditorWindow : CodeBuilderEditorWindow
    {
        ///---------------------------------------------------------------------
        [MenuItem("Prateek/Window/Prateek Tools Window")]
        private static CodeBuilderEditorWindow CreateWindow()
        {
            var window = GetWindow<PrateekScriptBuilderEditorWindow>();
            window.Show();
            return window;
        }

        ///---------------------------------------------------------------------
        public static void AddBuilder(PrateekScriptBuilder builder)
        {
            var window = (PrateekScriptBuilderEditorWindow)CreateWindow();
            window.activeCodeBuilders.Add(builder);
            window.startMarkUp = EditorApplication.timeSinceStartup;
        }

        ///---------------------------------------------------------------------
        protected override void OnEnable()
        {
            base.OnEnable();

            // Reference to the root of the window.
            var root = rootVisualElement;
            var scrollRoot = new ScrollView();
            root.Add(scrollRoot);

            Dictionary<string, List<KeywordUsage>> hierarchy = new Dictionary<string, List<KeywordUsage>>();
            foreach (var action in ScriptActionRegistry.Actions)
            {
                AddScriptAction(hierarchy, action);
            }

            foreach (var list in hierarchy.Values)
            {
                list.Sort((a, b) =>
                {
                    return string.Compare(a.keyword, b.keyword);
                });
            }

            AddVisualElement(scrollRoot, hierarchy, string.Empty, "ROOT");
        }

        private void AddScriptAction(Dictionary<string, List<KeywordUsage>> hierarchy, ScriptAction action)
        {
            foreach (var keyword in action.KeywordUsages)
            {
                if (string.IsNullOrEmpty(keyword.keyword))
                {
                    continue;
                }

                hierarchy.SafeAdd(keyword.scope, keyword);
            }
        }

        private void AddVisualElement(VisualElement root, Dictionary<string, List<KeywordUsage>> hierarchy, string key, string foldoutKey)
        {
            if (hierarchy.ContainsKey(key))
            {
                var foldout = new Foldout() { text = string.IsNullOrEmpty(key) ? "Root Scope" : key, viewDataKey = $"{foldoutKey}.Foldout" };

                var children = hierarchy[key];
                foreach (var child in children)
                {
                    AddVisualElement(foldout, hierarchy, child.keyword, $"{foldoutKey}.{child.keyword}");
                }

                root.Add(foldout);
            }
            else
            {
                root.Add(new Label(key));
            }
        }


        private VisualElement AddScriptActions(Dictionary<string, KeywordUsage> keywords)
        {
            var foldout = new Foldout() { text = "Active script blocks", viewDataKey = "ScriptActionsFoldout" };

            var container = new VisualElement();
            container.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            var leftContainer  = new VisualElement();
            var rightContainer = new VisualElement();
            foreach (var action in ScriptActionRegistry.Actions)
            {
                leftContainer.Add(new Label(action.CodeBlock));
                rightContainer.Add(new Label(action.GetType().Name));

                foreach (var keyword in action.KeywordUsages)
                {
                    if (string.IsNullOrEmpty(keyword.keyword))
                    {
                        continue;
                    }

                    if (keywords.ContainsKey(keyword.keyword))
                    {
                        continue;
                    }

                    keywords.Add(keyword.keyword, keyword);
                }
            }

            container.Add(leftContainer);
            container.Add(new Label("    "));
            container.Add(rightContainer);
            foldout.Add(container);

            return foldout;
        }

        private VisualElement AddKeywords(Dictionary<string, KeywordUsage> keywords)
        {
            var foldout = new Foldout() { text = "Keywords", viewDataKey = "KeywordsFoldout" };
            foreach (KeywordUsage keywordUsage in keywords.Values)
            {
                foldout.Add(new Label(keywordUsage.keyword));
            }
            return foldout;
        }
    }
}

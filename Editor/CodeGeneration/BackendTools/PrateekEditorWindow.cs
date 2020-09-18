#define PRATEEK_ALLOW_INTERNAL_TOOLS
// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 22/03/2020
//
//  Copyright � 2017-2020 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_IFDEF-
//-----------------------------------------------------------------------------
#region Prateek Ifdefs

//Auto activate some of the prateek defines
#if UNITY_EDITOR

//Auto activate debug
#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion Prateek Ifdefs
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
namespace Prateek.Editor.CodeGeneration.BackendTools
{
    using System.Collections.Generic;
    using Prateek.Editor.CodeGeneration.CodeBuilder.RuntimeBuilder;
    using Prateek.Editor.Core.EditorPrefs;
    using Prateek.Runtime.Core.Helpers;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    ///-------------------------------------------------------------------------
    public partial class PrateekEditorWindow : EditorWindow
    {
        ///---------------------------------------------------------------------
        #region Fields
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 scrollPosition2 = Vector2.zero;
        private Prefs.Bools prateekRunInTestMode;
        private Prefs.Strings prateekUpdaterDir;
        private CodeBuilder scriptTemplateUpdater = null;

        private List<CodeBuilder> activeCodeBuilders = new List<CodeBuilder>();

#if PRATEEK_ALLOW_INTERNAL_TOOLS
        private CodeBuilder prateekScriptGenerator = null;
        private Prefs.Strings prateekExportDir;
        private Prefs.Strings prateekSourceDir;
        private Prefs.ListStrings prateekSourceDir0;
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Unity Defaults
        //[MenuItem("Prateek/Window/Prateek Tools Window")]
        //private static PrateekEditorWindow CreateWindow()
        //{
        //    var window = (PrateekEditorWindow)EditorWindow.GetWindow(typeof(PrateekEditorWindow));
        //    window.Show();
        //    return window;
        //}

        /////---------------------------------------------------------------------
        //public static void AddBuilder(CodeBuilder builder)
        //{
        //    var window = CreateWindow();
        //    builder.StartWork();
        //    window.activeCodeBuilders.Add(builder);
        //}

        ///---------------------------------------------------------------------
        private void Awake()
        {
            InitDatas();
        }

        ///---------------------------------------------------------------------
        private void OnEnable()
        {
            // Reference to the root of the window.
            var root = rootVisualElement;

            var foldout = new Foldout() {text = "machin", viewDataKey = "foldout"};
            //var files = new Button() { text = "" };
            //foldout.Add(new Button() { text = "My Button" });
            //foldout.Add(new Button() { text = "My Button" });
            //foldout.Add(new Button() { text = "My Button" });

            // Creates our button and sets its Text property.
            var reloadPrateekScript = new Button() { text = "Reload" };
            reloadPrateekScript.clicked += () =>
            {
                prateekScriptGenerator = null;
                InitDatas();

                foldout.Clear();
                for (int w = 0; w < prateekScriptGenerator.WorkFileCount; w++)
                {
                    var label = new Label();
                    label.text = " - " + prateekScriptGenerator[w].source.name.Extension(prateekScriptGenerator[w].source.extension);
                    foldout.Add(label);
                }
            };

            //// Gives it some style.
            //myButton.style.width = 160;
            //myButton.style.height = 30;

            // Adds it to the root.
            root.Add(reloadPrateekScript);
            root.Add(foldout);
            root.Add(new Button() { text = "other" });
            root.Add(new TextField() { viewDataKey = "testTextField"});
        }



        ///---------------------------------------------------------------------
        private void OnDestroy() { }

        ///---------------------------------------------------------------------
        ///-- Keyboard focus ---------------------------------------------------
        private void OnFocus()
        {
        }

        ///---------------------------------------------------------------------
        private void OnLostFocus() { }

        ///---------------------------------------------------------------------
        ///-- Sent when an object or group of objects in the hierarchy changes -
        private void OnHierarchyChange() { }

        ///---------------------------------------------------------------------
        ///-- Sent whenever the state of the project changes -------------------
        private void OnProjectChange() { }

        ///---------------------------------------------------------------------
        ///-- Called whenever the selection has changed ------------------------
        private void OnSelectionChange() { }

        ///---------------------------------------------------------------------
        /// Called at 10 frames per second
        private void OnInspectorUpdate()
        {
            rootVisualElement.SetEnabled(true);
            if (activeCodeBuilders.Count > 0)
            {
                activeCodeBuilders.RemoveAll((x) => { return x == null; });
                for (int b = 0; b < activeCodeBuilders.Count; b++)
                {
                    var builder = activeCodeBuilders[b];
                    if (!builder.IsWorking)
                    {
                        rootVisualElement.SetEnabled(false);
                        builder.Update();
                    }
                }
            }

            //if (prateekScriptGenerator != null && prateekScriptGenerator.IsWorking)
            //{
            //    prateekScriptGenerator.Update();
            //}
        }

        ///---------------------------------------------------------------------
        private void Update() { }

        ///---------------------------------------------------------------------
#if PRATEEK_ALLOW_INTERNAL_TOOLS
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        private void OnGUI()
        {
            return;

            InitDatas();


            prateekUpdaterDir.Value = EditorGUILayout.TextField("Updater dir", prateekUpdaterDir.Value);
            if (scriptTemplateUpdater == null)
            {
                EditorGUILayout.LabelField("Source script folder not found");
            }
            else
            {
                if (GUI.Button(EditorGUILayout.GetControlRect(), "Execute updater"))
                {
                    scriptTemplateUpdater.RunInTestMode = prateekRunInTestMode.Value;
                    scriptTemplateUpdater.StartWork();
                }

                EditorGUILayout.LabelField("File count: " + scriptTemplateUpdater.WorkFileCount);
                using (var scrollScope = new EditorGUILayout.ScrollViewScope(scrollPosition, GUILayout.MaxHeight(350)))
                {
                    for (int w = 0; w < scriptTemplateUpdater.WorkFileCount; w++)
                    {
                        EditorGUILayout.LabelField(" - " + Strings.Extension(scriptTemplateUpdater[w].source.name, scriptTemplateUpdater[w].source.extension));
                    }
                    scrollPosition = scrollScope.scrollPosition;
                }
            }

            return;
#if PRATEEK_ALLOW_INTERNAL_TOOLS
            EditorGUILayout.Space();
            
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Reload"))
            {
                prateekScriptGenerator = null;
                InitDatas();
            }

            using (new EditorGUI.DisabledScope(prateekScriptGenerator != null && prateekScriptGenerator.IsWorking))
            {
                prateekRunInTestMode.Value = EditorGUILayout.ToggleLeft("Run in test mode", prateekRunInTestMode.Value);

                if (GUI.Button(EditorGUILayout.GetControlRect(), "Execute generator"))
                {
                    prateekScriptGenerator.RunInTestMode = prateekRunInTestMode.Value;
                    prateekScriptGenerator.StartWork();
                }

                prateekExportDir.Value = EditorGUILayout.TextField("Export dir", prateekExportDir.Value);
                prateekSourceDir.Value = EditorGUILayout.TextField("Source dir", prateekSourceDir.Value);

                {
                    if (GUI.Button(EditorGUILayout.GetControlRect(), "Add folder"))
                    {
                        prateekSourceDir0.Add(string.Empty);
                    }

                    if (GUI.Button(EditorGUILayout.GetControlRect(), "Remove Folder"))
                    {
                        prateekSourceDir0.RemoveLast();
                    }

                    for (int v = 0; v < prateekSourceDir0.Count; v++)
                    {
                        prateekSourceDir0[v] = EditorGUILayout.TextField(prateekSourceDir0[v]);
                    }
                }

                EditorGUILayout.LabelField("File count: " + prateekScriptGenerator.WorkFileCount);
                using (var scrollScope = new EditorGUILayout.ScrollViewScope(scrollPosition2, GUILayout.MaxHeight(350)))
                {
                    for (int w = 0; w < prateekScriptGenerator.WorkFileCount; w++)
                    {
                        EditorGUILayout.LabelField(" - " + prateekScriptGenerator[w].source.name.Extension(prateekScriptGenerator[w].source.extension));
                    }

                    scrollPosition2 = scrollScope.scrollPosition;
                }
            }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        }
        #endregion Unity Defaults

        ///---------------------------------------------------------------------
        #region Behaviour
        private void InitDatas()
        {
            if (prateekUpdaterDir == null)
                prateekUpdaterDir = Prefs.Get(GetType().Name + ".prateekUpdaterDir", "/Scripts");

            if (prateekRunInTestMode == null)
                prateekRunInTestMode = Prefs.Get(GetType().Name + ".prateekRunInTestMode", false);

            if (scriptTemplateUpdater == null)
            {
                scriptTemplateUpdater = Tools.GetScriptTemplateUpdater(prateekUpdaterDir.Value);
                if (scriptTemplateUpdater != null)
                    scriptTemplateUpdater.Init();
            }

#if PRATEEK_ALLOW_INTERNAL_TOOLS
            if (prateekExportDir == null)
                prateekExportDir = Prefs.Get(GetType().Name + ".prateekExportDir", string.Empty);

            if (prateekSourceDir == null)
                prateekSourceDir = Prefs.Get(GetType().Name + ".prateekSourceDir", string.Empty);

            if (prateekSourceDir0 == null)
                prateekSourceDir0 = Prefs.Get(GetType().Name + ".prateekSourceDir0", (List<string>)null);

            if (prateekScriptGenerator == null)
            {
                prateekScriptGenerator = Tools.GetPrateekScriptGenerator();
                prateekScriptGenerator.Init();
            }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        }
        #endregion Behaviour
    }
}

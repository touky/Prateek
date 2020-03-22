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
namespace Prateek.CodeGeneration.Editors
{
    using Prateek.Editors;
    using Prateek.Helpers;
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public partial class PrateekEditorWindow : EditorWindow
    {
        //---------------------------------------------------------------------
        #region Declarations
        #endregion Declarations

        //---------------------------------------------------------------------
        #region Settings
        #endregion Settings

        //---------------------------------------------------------------------
        #region Fields
        private Vector2 scrollPosition = Vector2.zero;
        private Vector2 scrollPosition2 = Vector2.zero;
        private Prefs.Bools prateekRunInTestMode;
        private Prefs.Strings prateekUpdaterDir;
        private CodeBuilder scriptTemplateUpdater = null;

#if PRATEEK_ALLOW_INTERNAL_TOOLS
        private CodeBuilder prateekScriptGenerator = null;
        private Prefs.Strings prateekExportDir;
        private Prefs.Strings prateekSourceDir;
        private Prefs.ListStrings prateekSourceDir0;
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        #endregion Properties

        //---------------------------------------------------------------------
        #region Unity Defaults
        [MenuItem("Prateek/Window/Prateek Tools Window")]
        static void CreateWindow()
        {
            var window = (PrateekEditorWindow)EditorWindow.GetWindow(typeof(PrateekEditorWindow));
            window.Show();
        }

        //---------------------------------------------------------------------
        private void Awake()
        {
            InitDatas();
        }

        //---------------------------------------------------------------------
        private void OnDestroy() { }

        //---------------------------------------------------------------------
        //-- Keyboard focus ---------------------------------------------------
        private void OnFocus()
        {
        }

        //---------------------------------------------------------------------
        private void OnLostFocus() { }

        //---------------------------------------------------------------------
        //-- Sent when an object or group of objects in the hierarchy changes -
        private void OnHierarchyChange() { }

        //---------------------------------------------------------------------
        //-- Sent whenever the state of the project changes -------------------
        private void OnProjectChange() { }

        //---------------------------------------------------------------------
        //-- Called whenever the selection has changed ------------------------
        private void OnSelectionChange() { }

        //---------------------------------------------------------------------
        // Called at 10 frames per second
        private void OnInspectorUpdate() { }

        //---------------------------------------------------------------------
        private void Update() { }

        //---------------------------------------------------------------------
#if PRATEEK_ALLOW_INTERNAL_TOOLS
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        private void OnGUI()
        {
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
                        EditorGUILayout.LabelField(" - " + scriptTemplateUpdater[w].source.name.Extension(scriptTemplateUpdater[w].source.extension));
                    }
                    scrollPosition = scrollScope.scrollPosition;
                }
            }

#if PRATEEK_ALLOW_INTERNAL_TOOLS
            EditorGUILayout.Space();
            if (GUI.Button(EditorGUILayout.GetControlRect(), "Reload"))
            {
                prateekScriptGenerator = null;
                InitDatas();
            }
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
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        }
        #endregion Unity Defaults

        //---------------------------------------------------------------------
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
                prateekScriptGenerator = Tools.GetPrateekScriptGenerator(prateekExportDir.Value, new List<string>(new string[] { prateekSourceDir.Value }));
                prateekScriptGenerator.Init();
            }
#endif //PRATEEK_ALLOW_INTERNAL_TOOLS
        }
        #endregion Behaviour
    }
}

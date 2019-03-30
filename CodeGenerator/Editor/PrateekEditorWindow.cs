// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 30/03/2019
//
//  Copyright � 2017-2019 "Touky" <touky@prateek.top>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//
// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
//
#region C# Prateek Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#region System
using System;
using System.Collections;
using System.Collections.Generic;
#endregion System

#region Unity
using Unity.Jobs;
using Unity.Collections;

#region Engine
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING
#endregion Engine

#region Editor
#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Editor
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;
using Prateek.Manager;

#region Using static
using static Prateek.ShaderTo.CSharp;
#endregion Using static

#region Editor
#if UNITY_EDITOR
using Prateek.CodeGeneration;
#endif //UNITY_EDITOR
#endregion Editor

#if PRATEEK_DEBUGS
using Prateek.Debug;
using static Prateek.Debug.Draw.Setup.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
using Prateek.Editors;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.CodeGeneration.Editor
{
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
        private CodeBuilder scriptTemplateUpdater = null;
        private CodeBuilder prateekScriptGenerator = null;
        private Prefs.Strings prateekExportDir;
        private Prefs.Strings prateekSourceDir;
        private Prefs.Bools prateekRunInTestMode;
        private Prefs.ListStrings prateekSourceDir0;
        #endregion Fields

        //---------------------------------------------------------------------
        #region Properties
        #endregion Properties

        //---------------------------------------------------------------------
        #region Unity Defaults
        [MenuItem("Prateek/Prateek Tools Window")]
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
        private void OnGUI()
        {
            InitDatas();

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
        }
        #endregion Unity Defaults

        //---------------------------------------------------------------------
        #region Behaviour
        private void InitDatas()
        {
            if (scriptTemplateUpdater == null)
            {
                scriptTemplateUpdater = Tools.GetScriptTemplateUpdater();
                scriptTemplateUpdater.Init();
            }

            if (prateekExportDir == null)
                prateekExportDir = Prefs.Get(GetType().Name + ".prateekExportDir", string.Empty);

            if (prateekSourceDir == null)
                prateekSourceDir = Prefs.Get(GetType().Name + ".prateekSourceDir", string.Empty);

            if (prateekRunInTestMode == null)
                prateekRunInTestMode = Prefs.Get(GetType().Name + ".prateekRunInTestMode", false);

            if (prateekSourceDir0 == null)
                prateekSourceDir0 = Prefs.Get(GetType().Name + ".prateekSourceDir0", (List<string>)null);

            if (prateekScriptGenerator == null)
            {
                prateekScriptGenerator = Tools.GetPrateekScriptGenerator(prateekExportDir.Value, new List<string>(new string[] { prateekSourceDir.Value }));
                prateekScriptGenerator.Init();
            }
        }
        #endregion Behaviour
    }
}

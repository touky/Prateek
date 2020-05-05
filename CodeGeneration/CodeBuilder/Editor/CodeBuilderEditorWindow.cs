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
namespace Prateek.CodeGeneration.BackendTools.Editor
{
    using System;
    using Prateek.CodeGeneration.CodeBuilder.Editor.CodeBuilder;
    using Prateek.Core.Code.Helpers;
    using Prateek.Core.Editor.EditorPrefs;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;

    ///-------------------------------------------------------------------------
    public class CodeBuilderEditorWindow : EditorWindow
    {
        ///---------------------------------------------------------------------
        #region Fields
        private List<CodeBuilder> activeCodeBuilders = new List<CodeBuilder>();

        private double startDuration = 0.65f;
        private double startMarkUp = -1;
        private bool isWorking = false;

        private Toggle runInTestMode;
        #endregion Fields

        ///---------------------------------------------------------------------
        [MenuItem("Prateek/Window/Prateek Tools Window")]
        private static CodeBuilderEditorWindow CreateWindow()
        {
            var window = GetWindow<CodeBuilderEditorWindow>();
            window.Show();
            return window;
        }

        ///---------------------------------------------------------------------
        public static void AddBuilder(CodeBuilder builder)
        {
            var window = CreateWindow();
            window.activeCodeBuilders.Add(builder);
            window.startMarkUp = EditorApplication.timeSinceStartup;
        }

        ///---------------------------------------------------------------------
        private void OnEnable()
        {
            // Reference to the root of the window.
            var root = rootVisualElement;

            var settingsFoldout = new Foldout() {text = "Settings", viewDataKey = "settingsFoldout"};
            runInTestMode = new Toggle("Run in test mode") {viewDataKey = "runInTestMode"};

            settingsFoldout.Add(runInTestMode);

            var foldout = new Foldout() {text = "machin", viewDataKey = "foldout"};
            //var files = new Button() { text = "" };
            //foldout.Add(new Button() { text = "My Button" });
            //foldout.Add(new Button() { text = "My Button" });
            //foldout.Add(new Button() { text = "My Button" });

            // Creates our button and sets its Text property.
            var reloadPrateekScript = new Button() { text = "Reload" };
            reloadPrateekScript.clicked += () =>
            {
                //prateekScriptGenerator = null;
                //InitDatas();

                //foldout.Clear();
                //for (int w = 0; w < prateekScriptGenerator.WorkFileCount; w++)
                //{
                //    var label = new Label();
                //    label.text = " - " + prateekScriptGenerator[w].source.name.Extension(prateekScriptGenerator[w].source.extension);
                //    foldout.Add(label);
                //}
            };

            //// Gives it some style.
            //myButton.style.width = 160;
            //myButton.style.height = 30;

            // Adds it to the root.
            root.Add(settingsFoldout);
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
            var startWork = false;
            if (startMarkUp > 0)
            {
                var progress = Math.Max(0, Math.Min(1, (EditorApplication.timeSinceStartup - startMarkUp) / startDuration));
                if (EditorUtility.DisplayCancelableProgressBar("Work start in:", string.Empty, (float)progress))
                {
                    startMarkUp = -1;
                    EditorUtility.ClearProgressBar();
                }
                else if (progress >= 1)
                {
                    startWork = true;
                }
            }

            if (startWork)
            {
                startMarkUp = -1;
                EditorUtility.ClearProgressBar();

                foreach (var builder in activeCodeBuilders)
                {
                    builder.RunInTestMode = runInTestMode.value;
                    builder.Init();
                    builder.StartWork();
                }

                isWorking = true;
                AssetDatabase.DisallowAutoRefresh();
            }

            if (isWorking)
            {
                isWorking = false;
                rootVisualElement.SetEnabled(true);
                if (activeCodeBuilders.Count > 0)
                {
                    activeCodeBuilders.RemoveAll((x) => { return x == null; });
                    for (int b = 0; b < activeCodeBuilders.Count; b++)
                    {
                        var builder = activeCodeBuilders[b];
                        if (builder.IsWorking)
                        {
                            rootVisualElement.SetEnabled(false);
                            builder.Update();
                            isWorking = true;
                        }
                    }
                }

                if (!isWorking)
                {
                    AssetDatabase.AllowAutoRefresh();
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
            }
        }
    }
}




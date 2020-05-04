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
namespace Prateek.FrameRecorder.Editor
{
    using System;
    using Prateek.Core.Code.Extensions;
    using Prateek.Core.Editor.EditorPrefs;
    using Prateek.Core.Editor.GUI;
    using Prateek.FrameRecorder.Code;
    using UnityEditor;
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public partial class FrameRecorderEditorWindow : EditorWindow
    {
        ///---------------------------------------------------------------------
        #region Declarations
        #endregion Declarations

        ///---------------------------------------------------------------------
        #region Settings
        #endregion Settings

        ///---------------------------------------------------------------------
        #region Fields
        private Prefs.Ints maxFrameRecorded;
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Properties
        #endregion Properties

        ///---------------------------------------------------------------------
        #region Unity Defaults
        [MenuItem("Prateek/Window/FrameRecorderEditorWindow")]
        static void CreateWindow()
        {
            var window = (FrameRecorderEditorWindow)EditorWindow.GetWindow(typeof(FrameRecorderEditorWindow));
            window.Show();
        }

        ///---------------------------------------------------------------------
        private void OnDestroy() { }

        ///---------------------------------------------------------------------
        ///-- Keyboard focus ---------------------------------------------------
        private void OnFocus() { }

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
        // Called at 10 frames per second
        private void OnInspectorUpdate() { }

        ///---------------------------------------------------------------------
        private void TryInit()
        {
            if (maxFrameRecorded == null)
            {
                maxFrameRecorded = new Prefs.Ints("FrameRecorderEditorWindow.maxFrameRecorded", 50);
            }
        }

        ///---------------------------------------------------------------------
        private void Update()
        {
            var isRecord = FrameRecorder.State == FrameRecorderManager.StateType.Recording;
            var isPlayback = FrameRecorder.State == FrameRecorderManager.StateType.Playback;
            if (isRecord || isPlayback)
                Repaint();


            //todo if (EditorApplication.isPaused)
            //todo     TickableRegistry.GetManager<FrameRecorderManager>().OnFakeUpdate();
        }

        ///---------------------------------------------------------------------
        private void OnGUI()
        {
            TryInit();

            var isRecord = FrameRecorder.State == FrameRecorderManager.StateType.Recording;
            var isPlayback = FrameRecorder.State == FrameRecorderManager.StateType.Playback;
            using (var enableScope = new GUIStatusScope(Application.isPlaying))
            {
                using (new EditorGUILayout.HorizontalScope(GUILayout.Height(40)))
                {
                    var newState = FrameRecorder.State;

                    using (new GUIStatusScope(!isPlayback))
                    {
                        if (!isRecord)
                        {
                            newState = GUILayout.Button("Record") ? FrameRecorderManager.StateType.Recording : newState;
                        }
                        else
                        {
                            newState = GUILayout.Button("Stop Record") ? FrameRecorderManager.StateType.Inactive : newState;
                        }
                    }

                    {
                        if (!isPlayback)
                        { newState = GUILayout.Button("|> Playback") ? FrameRecorderManager.StateType.Playback : newState; }
                        else
                        { newState = GUILayout.Button("Stop playback") ? FrameRecorderManager.StateType.Inactive : newState; }
                    }

                    if (GUILayout.Button("X Clear History \u262D"))
                    {
                        FrameRecorder.ClearHistory();
                    }

                    FrameRecorder.State = newState;
                }

                {
                    var range = FrameRecorder.CurrentFrameRange;
                    var frameCount = FrameRecorder.FrameCount;
                    var recordLimit = maxFrameRecorded.Value;
                    var rX = (float)range.x;
                    var rY = (float)range.y;
                    var size = rY - rX;
                    using (new GUIStatusScope(frameCount > 0 && isPlayback))
                    {
                        var zone = EditorGUILayout.GetControlRect();
                        var width = CSharp.max(0, zone.width - 55);
                        {
                            zone.width = width;
                            using (new GUIStatusScope(Color.black))
                            { GUI.Box(zone, GUIContent.none); }

                            if (frameCount > 0)
                            {
                                zone.width = width * ((float)frameCount / (float)recordLimit);
                                using (new GUIStatusScope(new Color(0, 0.6f, 0)))
                                { GUI.Box(zone, GUIContent.none); }

                                zone.x += width * ((float)rX / (float)recordLimit);
                                zone.width = width * ((float)(size + 1) / (float)recordLimit);
                                using (new GUIStatusScope(Color.green))
                                { GUI.Box(zone, GUIContent.none); }
                            }
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            var oldX = rX;
                            rX = CSharp.min(recordLimit - (size + 1), EditorGUILayout.IntSlider((int)rX, 0, recordLimit - 1));
                            rY += rX - oldX;
                        }

                        using (new EditorGUILayout.HorizontalScope())
                        {
                            GUILayout.Label("Position:", GUILayout.MaxWidth(60));

                            var offset = 0;
                            offset -= GUILayout.Button("|<", GUILayout.MaxWidth(40)) ? 1 : 0;
                            rX = EditorGUILayout.IntField((int)rX, GUILayout.MaxWidth(40));
                            offset += GUILayout.Button(">|", GUILayout.MaxWidth(40)) ? 1 : 0;
                            rX += offset;
                            rY += offset;

                            EditorGUILayout.GetControlRect(GUILayout.Width(40));

                            GUILayout.Label("Range: ", GUILayout.MaxWidth(40));
                            size = rY - rX;
                            size -= GUILayout.Button("|<", GUILayout.MaxWidth(40)) ? 1 : 0;
                            size = CSharp.max(0, EditorGUILayout.IntField((int)size, GUILayout.MaxWidth(40)));
                            size += GUILayout.Button(">|", GUILayout.MaxWidth(40)) ? 1 : 0;
                            rY = rX + size;

                            EditorGUILayout.GetControlRect(GUILayout.Width(40));

                            GUILayout.Label(String.Format("Frame count: {0} / ", frameCount), GUILayout.Width(100));
                            recordLimit = EditorGUILayout.IntField(recordLimit, GUILayout.MaxWidth(80));
                        }
                    }
                    range.x = (int)rX;
                    range.y = (int)rY;
                    maxFrameRecorded.Value = recordLimit;
                    FrameRecorder.MaxFrameRecorded = recordLimit;
                    FrameRecorder.CurrentFrameRange = range;
                }
            }
        }
        #endregion Unity Defaults

        ///---------------------------------------------------------------------
        #region Behaviour
        #endregion Behaviour
    }
}

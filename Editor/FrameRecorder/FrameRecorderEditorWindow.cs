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
namespace Prateek.Editor.FrameRecorder
{
    using System;
    using Prateek.Editor.Core.EditorPrefs;
    using Prateek.Editor.Core.GUI;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.FrameRecorder;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using static Prateek.Runtime.Core.Extensions.Statics;

    ///-------------------------------------------------------------------------
    public partial class FrameRecorderEditorWindow : EditorWindow
    {
        #region Fields
        ///---------------------------------------------------------------------
        private Prefs.Ints maxFrameRecorded;
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Unity Defaults
        [MenuItem("Prateek/Window/Frame Recorder Window")]
        private static void CreateWindow()
        {
            var window = GetWindow<FrameRecorderEditorWindow>();
            window.Show();
        }

        ///---------------------------------------------------------------------
        private void TryInit()
        {
            if (maxFrameRecorded == null)
            {
                maxFrameRecorded = new Prefs.Ints("FrameRecorderEditorWindow.maxFrameRecorded", 50);
            }
        }

        ///---------------------------------------------------------------------
        private VisualElement rootElement;

        private class test : IBinding
        {
            private int i;
            public void PreUpdate()
            {
            }

            public void Release()
            {
            }

            public void Update()
            {
            }
        }

        private Label frameLabel;
        private Toggle showMultipleFrames;
        private SliderInt previewSingle;
        private MinMaxSlider previewMultiple;
        protected virtual void OnEnable()
        {
            // Reference to the root of the window.
            rootElement = rootVisualElement;
            
            frameLabel = new Label("Frame shown:");
            previewSingle = new SliderInt(0, 100) {viewDataKey = "previewSingle"};
            previewMultiple = new MinMaxSlider(0, 100, 0, 100) {viewDataKey = "previewMultiple"};
            showMultipleFrames = new Toggle("Show multiple frames ?") { viewDataKey = "showMultipleFrames"};

            rootElement.Add(frameLabel);
            rootElement.Add(previewSingle);
            rootElement.Add(previewMultiple);
            rootElement.Add(showMultipleFrames);
        }

        private double timer;
        private void Update()
        {
            //if (!EditorApplication.isPlaying)
            //{
            //    rootElement.SetEnabled(false);
            //    return;
            //}

            rootElement.SetEnabled(true);

            if (showMultipleFrames.value)
            {
                previewMultiple.visible = false;
                previewSingle.visible = true;
                frameLabel.text = $"Frame previewed: {previewSingle.value}";
            }
            else
            {
                previewMultiple.visible= true;
                previewSingle.visible = false;
                previewMultiple.value = vec2i((int)previewMultiple.value.x, (int)previewMultiple.value.y);
                frameLabel.text = $"Frame previewed: [{previewMultiple.value.x}, {previewMultiple.value.y}]";
            }
        }

        /////---------------------------------------------------------------------
        //private void Update()
        //{
        //    var isRecord = FrameRecorder__.RecorderState == RecorderState.Recording;
        //    var isPlayback = FrameRecorder__.RecorderState == RecorderState.Playback;
        //    if (isRecord || isPlayback)
        //        Repaint();


        //    //todo if (EditorApplication.isPaused)
        //    //todo     TickableRegistry.GetManager<FrameRecorderManager>().OnFakeUpdate();
        //}

        /////---------------------------------------------------------------------
        //private void OnGUI()
        //{
        //    TryInit();

        //    var isRecord = FrameRecorder__.RecorderState == RecorderState.Recording;
        //    var isPlayback = FrameRecorder__.RecorderState == RecorderState.Playback;
        //    using (var enableScope = new GUIStatusScope(Application.isPlaying))
        //    {
        //        using (new EditorGUILayout.HorizontalScope(GUILayout.Height(40)))
        //        {
        //            var newState = FrameRecorder__.RecorderState;

        //            using (new GUIStatusScope(!isPlayback))
        //            {
        //                if (!isRecord)
        //                {
        //                    newState = GUILayout.Button("Record") ? RecorderState.Recording : newState;
        //                }
        //                else
        //                {
        //                    newState = GUILayout.Button("Stop Record") ? RecorderState.Inactive : newState;
        //                }
        //            }

        //            {
        //                if (!isPlayback)
        //                { newState = GUILayout.Button("|> Playback") ? RecorderState.Playback : newState; }
        //                else
        //                { newState = GUILayout.Button("Stop playback") ? RecorderState.Inactive : newState; }
        //            }

        //            if (GUILayout.Button("X Clear History \u262D"))
        //            {
        //                FrameRecorder__.ClearHistory();
        //            }

        //            FrameRecorder__.RecorderState = newState;
        //        }

        //        {
        //            var range = FrameRecorder__.CurrentFrameRange;
        //            var frameCount = FrameRecorder__.FrameCount;
        //            var recordLimit = maxFrameRecorded.Value;
        //            var rX = (float)range.x;
        //            var rY = (float)range.y;
        //            var size = rY - rX;
        //            using (new GUIStatusScope(frameCount > 0 && isPlayback))
        //            {
        //                var zone = EditorGUILayout.GetControlRect();
        //                var width = max(0, zone.width - 55);
        //                {
        //                    zone.width = width;
        //                    using (new GUIStatusScope(Color.black))
        //                    { GUI.Box(zone, GUIContent.none); }

        //                    if (frameCount > 0)
        //                    {
        //                        zone.width = width * ((float)frameCount / (float)recordLimit);
        //                        using (new GUIStatusScope(new Color(0, 0.6f, 0)))
        //                        { GUI.Box(zone, GUIContent.none); }

        //                        zone.x += width * ((float)rX / (float)recordLimit);
        //                        zone.width = width * ((float)(size + 1) / (float)recordLimit);
        //                        using (new GUIStatusScope(Color.green))
        //                        { GUI.Box(zone, GUIContent.none); }
        //                    }
        //                }

        //                using (new EditorGUILayout.HorizontalScope())
        //                {
        //                    var oldX = rX;
        //                    rX = min(recordLimit - (size + 1), EditorGUILayout.IntSlider((int)rX, 0, recordLimit - 1));
        //                    rY += rX - oldX;
        //                }

        //                using (new EditorGUILayout.HorizontalScope())
        //                {
        //                    GUILayout.Label("Position:", GUILayout.MaxWidth(60));

        //                    var offset = 0;
        //                    offset -= GUILayout.Button("|<", GUILayout.MaxWidth(40)) ? 1 : 0;
        //                    rX = EditorGUILayout.IntField((int)rX, GUILayout.MaxWidth(40));
        //                    offset += GUILayout.Button(">|", GUILayout.MaxWidth(40)) ? 1 : 0;
        //                    rX += offset;
        //                    rY += offset;

        //                    EditorGUILayout.GetControlRect(GUILayout.Width(40));

        //                    GUILayout.Label("Range: ", GUILayout.MaxWidth(40));
        //                    size = rY - rX;
        //                    size -= GUILayout.Button("|<", GUILayout.MaxWidth(40)) ? 1 : 0;
        //                    size = max(0, EditorGUILayout.IntField((int)size, GUILayout.MaxWidth(40)));
        //                    size += GUILayout.Button(">|", GUILayout.MaxWidth(40)) ? 1 : 0;
        //                    rY = rX + size;

        //                    EditorGUILayout.GetControlRect(GUILayout.Width(40));

        //                    GUILayout.Label(String.Format("Frame count: {0} / ", frameCount), GUILayout.Width(100));
        //                    recordLimit = EditorGUILayout.IntField(recordLimit, GUILayout.MaxWidth(80));
        //                }
        //            }
        //            range.x = (int)rX;
        //            range.y = (int)rY;
        //            maxFrameRecorded.Value = recordLimit;
        //            FrameRecorder__.MaxFrameRecorded = recordLimit;
        //            FrameRecorder__.CurrentFrameRange = range;
        //        }
        //    }
        //}
        #endregion Unity Defaults
    }
}

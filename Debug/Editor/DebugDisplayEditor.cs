// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 18/03/19
//
//  Copyright © 2017-2019 "Touky" <touky@prateek.top>
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
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.Serialization;

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Unity.Jobs;
using Unity.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR
#endregion Unity

#region Prateek
using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUGS
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Editors
{
    //-------------------------------------------------------------------------
#if PRATEEK_DEBUGS
    public class DrawEditorBase : EditorWindow
    {
        private struct enumData
        {
            public int value;
            public string name;
        }

        private List<enumData> m_enum_data = new List<enumData>();
        private Vector2 m_scrollPosition;

        //---------------------------------------------------------------------
        public class GUISetup
        {
            public Font m_font;
            public GUIStyle m_background = null;
            public List<GUIStyle> m_item_background = null;
            public GUIStyle m_item_text = null;
            public GUIStyle[] m_active = null;

            public GUISetup()
            {
                GUIStyle style = null;
                var border1 = new RectOffset(1, 1, 1, 1);
                var border2 = new RectOffset(2, 2, 2, 2);
                var tex_size1 = new Rect(1, 1, 1, 1);
                var tex_size2 = new Rect(2, 2, 1, 1);

                m_font = m_font != null ? m_font : Helpers.Fonts.Get("Consolas", GUI.skin.font.fontSize);

                if (m_background == null)
                {
                    m_background = Helpers.GUIStyles.Get(GUI.skin.box, Vector2.zero, border2, 8, new Color(1f, 1f, 1f));
                    m_background.normal.background = Helpers.Textures.Make(tex_size2, new Color(0.3f, 0.3f, 0.3f), Color.black);
                }

                if (m_item_background == null)
                {
                    m_item_background = new List<GUIStyle>();

                    float color = 0.8f;
                    for (int i = 0; i < 5; i++)
                    {
                        style = Helpers.GUIStyles.Get(String.Format("item_{0}_", i), GUI.skin.box, Vector2.zero, border1, 8, new Color(1f, 1f, 1f));
                        style.normal.background = Helpers.Textures.Make(tex_size1, new Color(color, color, color), Color.black);
                        m_item_background.Add(style);
                        color -= 0.05f;
                    }
                }

                if (m_item_text == null)
                {
                    m_item_text = Helpers.GUIStyles.Get("text", GUI.skin.label, Vector2.zero, border1, 10, Color.black);
                    m_item_text.alignment = TextAnchor.MiddleLeft;
                    m_item_text.padding = new RectOffset();
                }

                if (m_active == null)
                {
                    m_active = new GUIStyle[2];

                    m_active[0] = Helpers.GUIStyles.Get("active_off_", GUI.skin.box, Vector2.zero, border1, 8, new Color(1f, 1f, 1f));
                    m_active[0].normal.background = Helpers.Textures.Make(tex_size1, new Color(0.1f, 0.3f, 0.1f), Color.grey);

                    m_active[1] = Helpers.GUIStyles.Get("active_on__", GUI.skin.box, Vector2.zero, border1, 8, new Color(1f, 1f, 1f));
                    m_active[1].normal.background = Helpers.Textures.Make(tex_size1, new Color(0.1f, 1.0f, 0.1f), Color.black);
                }
            }
        }
        private GUISetup m_style;

        //---------------------------------------------------------------------
        protected virtual Type GetEnumType()
        {
            return null;
        }

        //---------------------------------------------------------------------
        void OnGUI()
        {
#region Main setup
            var enum_type = GetEnumType();
            if (enum_type == null)
                return;

            var manager = Base.Registry.instance.GetManager<DebugDisplayBase>();

            var enum_values = Enum.GetValues(enum_type);
            if (m_enum_data.Count != enum_values.Length)
            {
                m_enum_data.Clear();

                var enum_names = Enum.GetNames(enum_type);
                for (int i = 0; i < enum_values.Length; ++i)
                {
                    m_enum_data.Add(new enumData() { value = (int)enum_values.GetValue(i), name = enum_names[i] });
                }
            }

            if (m_style == null)
            {
                m_style = new GUISetup();
            }
#endregion Main setup

            m_scrollPosition = EditorGUILayout.BeginScrollView(m_scrollPosition);
            {
#region Draw setup
                var margin2 = Vector2.one * 2;
                var margin4 = Vector2.one * 4;
                var window_rect = EditorGUILayout.GetControlRect(GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * m_enum_data.Count + margin4.y * 2));
                var line_rect = window_rect;
                {
                    line_rect.x += margin4.x;
                    line_rect.width -= margin4.x * 2;
                    line_rect.height = EditorGUIUtility.singleLineHeight;
                    line_rect.y = window_rect.height - (line_rect.height + margin4.y);
                }
                var box_rect = line_rect;
                {
                    box_rect.width = line_rect.height - margin2.y * 2;
                    box_rect.height = line_rect.height - margin2.x * 2;
                    box_rect.x += margin2.x;
                    box_rect.y += margin2.y;
                }
                var text_rect = line_rect;
                {
                    text_rect.x += box_rect.width + margin2.x * 2;
                    text_rect.width -= box_rect.width + margin2.x * 3;
                    text_rect.height = EditorGUIUtility.singleLineHeight;
                }
#endregion Draw setup

#region Draw background
                GUI.Box(window_rect, GUIContent.none, m_style.m_background);
#endregion Draw background

#region Draw lines

                for (int i = m_enum_data.Count - 1; i >= 0; i--)
                {
                    var indent = manager != null ? manager.CountParents(i) : 0;
                    var old_x = line_rect.x;
                    var old_width = line_rect.width;
                    {
                        line_rect.x += indent * line_rect.height;
                        line_rect.width -= indent * line_rect.height;
                        box_rect.x += indent * line_rect.height;
                        text_rect.x += indent * line_rect.height;

                        GUI.Box(line_rect, GUIContent.none, m_style.m_item_background[Mathf.Min(indent, m_style.m_item_background.Count)]);
                        GUI.Box(box_rect, GUIContent.none, m_style.m_active[i % 2]);
                        GUI.Label(text_rect, m_enum_data[i].name, m_style.m_item_text);
                    }
                    line_rect.x = old_x;
                    line_rect.width = old_width;
                    line_rect.y -= line_rect.height;
                    box_rect.x = old_x + margin2.x;
                    box_rect.y -= line_rect.height;
                    text_rect.x = old_x + box_rect.width + margin2.x * 2;
                    text_rect.y -= line_rect.height;
                }

#endregion Draw lines
            }
            EditorGUILayout.EndScrollView();
            //m_enum_data.Sort((a, b) =>
            //{
            //    if (a.value == DebugDisplayType.OVERRIDE_DEACTIVATE_ALL)
            //        return -1;
            //    if (b.value == DebugDisplayType.OVERRIDE_DEACTIVATE_ALL)
            //        return 1;
            //    return String.Compare(a.name, b.name);
            //});

            return;

            /*
             * 
            //LOGGER ------------------------------------------------------------------------------
            var maxFrameRecorded = "DebugDisplayEditorMaxFrameRecorded";
            var showLogger = "DebugDisplayEditorShowLogger";
            FrameRecorder.MaxFrameRecorded = EditorPrefs.GetInt(maxFrameRecorded, FrameRecorder.MaxFrameRecorded);
            var wasLoggerVisible = EditorPrefs.GetBool(showLogger);
            var isLoggerVisible = EditorGUILayout.Toggle("Use DrawLine Logger", wasLoggerVisible);
            if (isLoggerVisible)
            {
                var oldGuiEnabled = GUI.enabled;
                var mainGuiEnabled = GUI.enabled && Application.isPlaying;
                {
                    FrameRecorder.State newState = wasLoggerVisible != isLoggerVisible ? FrameRecorder.State.Playback : FrameRecorder.status;
                    switch (FrameRecorder.status)
                    {
                        case FrameRecorder.State.Recording: { break; }
                        case FrameRecorder.State.Playback: { break; }
                        case FrameRecorder.State.PlaybackPaused: { break; }
                        default: { newState = FrameRecorder.State.Playback; break; }
                    }
                    bool clearAll = false;

                    //UI behaviour
                    EditorGUILayout.BeginHorizontal();
                    {
                        bool isRecord = FrameRecorder.status == FrameRecorder.State.Recording;
                        bool isPlayback = FrameRecorder.status == FrameRecorder.State.Playback
                                       || FrameRecorder.status == FrameRecorder.State.PlaybackPaused;
                        bool isPlaybackPaused = FrameRecorder.status == FrameRecorder.State.PlaybackPaused;

                        GUI.enabled = mainGuiEnabled && isPlayback;
                        newState = GUILayout.Button("O Record") ? FrameRecorder.State.Recording : newState;

                        GUI.enabled = mainGuiEnabled && isRecord;
                        newState = GUILayout.Button("|| Pause") ? FrameRecorder.State.PlaybackPaused : newState;

                        GUI.enabled = mainGuiEnabled && (isRecord || isPlaybackPaused);
                        newState = GUILayout.Button("X Stop") ? FrameRecorder.State.Playback : newState;

                        GUI.enabled = mainGuiEnabled && isPlayback && !isPlaybackPaused;
                        clearAll = GUILayout.Button("X Clear all \u262D");

                        if (isPlaybackPaused)
                        {
                            Draw.EndFrame();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    GUI.enabled = mainGuiEnabled;

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUI.enabled = mainGuiEnabled && FrameRecorder.frame_count > 0 && FrameRecorder.status >= FrameRecorder.State.Playback;
                        GUILayout.Label("Position:", GUILayout.MaxWidth(60));
                        FrameRecorder.CurrentFrameIndex = EditorGUILayout.IntSlider(FrameRecorder.CurrentFrameIndex, 0, Mathf.Max(1, FrameRecorder.frame_count - 1));
                        FrameRecorder.CurrentFrameIndex -= GUILayout.Button("|<", GUILayout.MaxWidth(40)) ? 1 : 0;
                        FrameRecorder.CurrentFrameIndex += GUILayout.Button(">|", GUILayout.MaxWidth(40)) ? 1 : 0;
                        GUI.enabled = oldGuiEnabled && FrameRecorder.status >= FrameRecorder.State.Playback;
                        GUILayout.Label("Max:", GUILayout.MaxWidth(40));
                        FrameRecorder.MaxFrameRecorded = EditorGUILayout.IntField(FrameRecorder.MaxFrameRecorded, GUILayout.MaxWidth(100));
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    GUI.enabled = mainGuiEnabled;

                    //Treat infos
                    FrameRecorder.status = newState;
                    if (clearAll)
                    {
                        FrameRecorder.ClearHistory();
                    }
                }
                GUI.enabled = oldGuiEnabled;
            }
            else
            {
                if (wasLoggerVisible != isLoggerVisible)
                {
                    FrameRecorder.status = FrameRecorder.State.Inactive;
                }
            }
            EditorPrefs.SetBool(showLogger, isLoggerVisible);
            EditorPrefs.SetInt(maxFrameRecorded, FrameRecorder.MaxFrameRecorded);

            //DEBUG DISPLAY -----------------------------------------------------------------------

            //var colors = new Color[enumValues.Length];
            //colors[(int)DebugType.Raycast]      = new Color(1, 1, 1, 1);
            //colors[(int)DebugType.Navigation]   = new Color(1, 1, 1, 1);
            //colors[(int)DebugType.Effects]      = new Color(1, 1, 1, 1);
            //colors[(int)DebugType.Electricity]  = new Color(1, 1, 1, 1);
            //colors[(int)DebugType.Robots]       = new Color(1, 1, 1, 1);
            //colors[(int)DebugType.Ash]          = new Color(1, 1, 1, 1);
            //colors[(int)DebugType.Physic]       = new Color(1, 1, 1, 1);

            m_scrollPosition = GUILayout.BeginScrollView(m_scrollPosition, false, true);

            GUILayout.BeginHorizontal();
            var backupColor = GUI.color;
            GUI.color = new Color(0.8f, 1.0f, 0.8f, 1.0f);
            EditorGUILayout.LabelField(new GUIContent("A", "On All Objects"), GUILayout.Width(12));

            GUI.color = new Color(0.8f, 1.0f, 0.8f, 1.0f);
            EditorGUILayout.LabelField(new GUIContent("S", "On Selected Objects"), GUILayout.Width(12));

            GUI.color = backupColor;
            EditorGUILayout.Space();
            GUILayout.Label("Duration", GUILayout.Width(60));
            var oldDuration = 0; //TODO DebugDisplay.Duration;
            var newDuration = EditorGUILayout.FloatField(GUIContent.none, oldDuration, GUILayout.Width(30));
            if (newDuration != oldDuration)
            {
                //TODO DebugDisplay.Duration = newDuration;
            }
            GUILayout.EndHorizontal();

            bool valueChanged = false;
            foreach (var d in m_enum_data)
            {
                var debugType = d.value;

                GUILayout.BeginHorizontal();

                var oldValue = false;
                var newValue = false;

                //oldValue = DebugDisplay.IsUnselectedActive(debugType);
                //GUI.color = new Color(0.8f, 0.8f, 0.8f, 1.0f);
                //newValue = EditorGUILayout.Toggle(oldValue, GUILayout.Width(12));
                //if (oldValue != newValue)
                //{
                //    valueChanged = true;
                //    DebugDisplay.SetActive(debugType, false, newValue);
                //}

                oldValue = DebugDisplay.IsActive(debugType);
                GUI.color = new Color(0.8f, 1.0f, 0.8f, 1.0f);
                newValue = EditorGUILayout.Toggle(oldValue, GUILayout.Width(12));
                GUI.color =  backupColor;

                if (oldValue != newValue)
                {
                    valueChanged = true;
                    DebugDisplay.SetActive(debugType, newValue);
                }

                GUILayout.Label(d.name);

                GUILayout.EndHorizontal();
            }

            if (valueChanged)
            {
                SceneView.RepaintAll();
            }

            GUILayout.EndScrollView();
            */
        }
    }
#endif //PRATEEK_DEBUGS
}

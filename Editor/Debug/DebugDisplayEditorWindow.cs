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
#if PRATEEK_DEBUG
namespace Prateek.Editor.Debug
{
    using System;
    using System.Collections.Generic;
    using Prateek.Editor.Core.EditorPrefs;
    using Prateek.Runtime.Core.Extensions;
    using Prateek.Runtime.Core.Helpers;
    using Prateek.Runtime.DebugFramework;
    using UnityEditor;
    using UnityEngine;
    using Statics = Prateek.Runtime.Core.Statics.Statics;

    ///-------------------------------------------------------------------------
    public class DebugDisplayEditorWindow : EditorWindow
    {
        ///---------------------------------------------------------------------
        #region Declarations
        public struct EnumInfos
        {
            ///-----------------------------------------------------------------
            public struct Data
            {
                public string name;
                public int value;
            }

            ///-----------------------------------------------------------------
            private Type type;
            private Data[] datas;

            ///-----------------------------------------------------------------
            public Type Type { get { return type; } }
            public int Count { get { return datas == null ? 0 : datas.Length; } }
            public Data this[int index] { get { return datas == null ? new Data() : datas[index]; } }

            ///-----------------------------------------------------------------
            public void Init(Type type)
            {
                if (this.type == type || !type.IsEnum)
                    return;

                this.type = type;

                var values = Enum.GetValues(type);
                var names = Enum.GetNames(type);
                datas = new Data[values.Length];

                for (int i = 0; i < values.Length; ++i)
                {
                    var value = values.GetValue(i);
                    datas[i] = new EnumInfos.Data()
                    {
                        value = (int)value,
                        name = names[i]
                    };
                }
            }

            ///-----------------------------------------------------------------
            public int Find(int value)
            {
                for (int d = 0; d < datas.Length; d++)
                {
                    if (datas[d].value == value)
                        return d;
                }
                return -1;
            }
        }

        ///---------------------------------------------------------------------
        public static class GUIDraw
        {
            ///-- Returns interior rect ----------------------------------------
            public static Rect BackgroundAuto(int lineCount, int lineMargin, int bgMargin, GUIStyle style)
            {
                var lineHeight = EditorGUIUtility.singleLineHeight;
                var rect = EditorGUILayout.GetControlRect(GUILayout.MinHeight((lineHeight + lineMargin * 2) * lineCount + bgMargin * 2));
                GUI.Box(rect, GUIContent.none, style);
                return rect.Inflate(-bgMargin);
            }

            ///-- Returns interior rect ----------------------------------------
            public static Rect Background(Rect rect, int margin = 0, GUIStyle style = null)
            {
                if (style != null)
                    GUI.Box(rect, GUIContent.none, style);

                return rect.Inflate(-margin);
            }

            ///-- Returns interior rect ----------------------------------------
            public static Rect Square(Rect rect, int margin = 0, GUIStyle style = null) { return Square(ref rect, margin, style); }
            public static Rect Square(ref Rect rect, int margin = 0, GUIStyle style = null)
            {
                var box = rect;
                box.width = Statics.min(box.width, box.height);
                box.width = box.height;

                if (rect.width == box.width)
                    rect = rect.TruncateY(rect.width);
                else if (rect.height == box.height)
                    rect = rect.TruncateX(rect.height);

                if (style != null)
                    GUI.Box(box, GUIContent.none, style);

                return box.Inflate(-margin);
            }
        }

        ///---------------------------------------------------------------------
        public class GUISetup
        {
            public Font font;
            public GUIStyle background = null;
            public List<GUIStyle> itemBG = null;
            public GUIStyle titleText = null;
            public GUIStyle itemText = null;
            public GUIStyle[] actives = null;

            public GUISetup()
            {
                GUIStyle style = null;
                var border1 = new RectOffset(1, 1, 1, 1);
                var border2 = new RectOffset(2, 2, 2, 2);
                var texSize1 = new Rect(1, 1, 1, 1);
                var texSize2 = new Rect(2, 2, 1, 1);

                font = font != null ? font : Fonts.Get("Consolas", GUI.skin.font.fontSize);

                if (background == null)
                {
                    background = GUIStyles.Get(GUI.skin.box, Vector2.zero, border2, 8, new Color(1f, 1f, 1f));
                    background.normal.background = Textures.Make(texSize2, new Color(0.3f, 0.3f, 0.3f), Color.black);
                }

                if (itemBG == null)
                {
                    itemBG = new List<GUIStyle>();

                    float color = 0.8f;
                    for (int i = 0; i < 5; i++)
                    {
                        style = GUIStyles.Get(String.Format("item_{0}_", i), GUI.skin.box, Vector2.zero, border1, 8, new Color(1f, 1f, 1f));
                        style.normal.background = Textures.Make(texSize1, new Color(color, color, color), Color.black);
                        itemBG.Add(style);
                        color -= 0.05f;
                    }
                }

                if (titleText == null)
                {
                    titleText = GUIStyles.Get("text", GUI.skin.label, Vector2.zero, border1, 18, Color.black);
                    titleText.fontStyle = FontStyle.Bold;
                    titleText.alignment = TextAnchor.MiddleCenter;
                    titleText.padding = new RectOffset();
                }

                if (itemText == null)
                {
                    itemText = GUIStyles.Get("text", GUI.skin.label, Vector2.zero, border1, 10, Color.black);
                    itemText.alignment = TextAnchor.MiddleLeft;
                    itemText.padding = new RectOffset();
                }

                if (actives == null)
                {
                    actives = new GUIStyle[2];

                    actives[0] = GUIStyles.Get("active_off_", GUI.skin.box, Vector2.zero, border1, 8, new Color(1f, 1f, 1f));
                    actives[0].normal.background = Textures.Make(texSize1, new Color(0.1f, 0.3f, 0.1f), Color.grey);

                    actives[1] = GUIStyles.Get("active_on__", GUI.skin.box, Vector2.zero, border1, 8, new Color(1f, 1f, 1f));
                    actives[1].normal.background = Textures.Make(texSize1, new Color(0.1f, 1.0f, 0.1f), Color.black);
                }

                //d.Init(new Color(194f / 255f, 0, 0).rrrn(1), 512, 512, new Rect(vec2(-10), vec2(20)), "test");
                //d.Clear();
                ////d.Add(new Textures.Drawer.Sphere(5) { color = Color.red, elongate = vec3(2, 0, 2) });
                ////d.Add(new Textures.Drawer.Sphere(5) { color = Color.black, fallout = 2.5f, skin = 0.1f, elongate = vec3(2, 0, 2) });
                //d.Add(new Textures.Drawer.Box(vec3(6, 2, 2)) { color = Color.red });
                //d.Add(new Textures.Drawer.SmoothSubstraction(new Textures.Drawer.Box(vec3(6, 2, 2)),
                //                                             new Textures.Drawer.Box(vec3(2, 4, 6)),
                //                                             1)
                //{ color = Color.cyan });
                ////d.Add(new Textures.Drawer.Torus(vec2(8, 1)) { color = Color.green, fallout = 1 });
                ////d.Add(new Textures.Drawer.Hexagon(vec2(3, 1)) { color = Color.magenta, fallout = 1 });
                ////d.Add(new Textures.Drawer.Triangle(vec3(0), vec3(2, 0, 0), vec3(1, 0, 2)) { color = Color.cyan, fallout = 1, rounding = 1 });
                //d.Make();

                //var txt = EditorGUILayout.GetControlRect(GUILayout.Width(d.texture.width), GUILayout.Height(d.texture.height));
                //GUI.Box(txt, d.texture);

            }
        }
        #endregion Declarations

        ///---------------------------------------------------------------------
        #region Settings
        #endregion Settings

        ///---------------------------------------------------------------------
        #region Fields
        private EnumInfos enumInfos;
        private Vector2 scrollPosition;

        private GUISetup styleSetup;

        private Prefs.ListBools activeFlags;
        private Prefs.ListBools expandedFlags;
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Unity Defaults
        [MenuItem("Prateek/Window/DebugDisplayEditorWindow")]
        static void CreateWindow()
        {
            var window = (DebugDisplayEditorWindow)EditorWindow.GetWindow(typeof(DebugDisplayEditorWindow));
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
            if (activeFlags == null)
            {
                activeFlags = new Prefs.ListBools(GetType().Name + ".activeFlags", null);
                expandedFlags = new Prefs.ListBools(GetType().Name + ".expandedFlags", null);
            }
        }

        ///---------------------------------------------------------------------
        private void Update() { }

        ///---------------------------------------------------------------------
        //private bool IsExpanded(int value, ref DebugDisplayRegistry.FlagHierarchy flagDatas)
        //{
        //    int parent = 0;
        //    while (flagDatas.GetParent(value, ref parent))
        //    {
        //        int i = enumInfos.Find(parent);
        //        if (i < 0)
        //            break;

        //        if (!expandedFlags[i])
        //            return false;
        //        value = parent;
        //    }
        //    return true;
        //}

        ///---------------------------------------------------------------------
        public Textures.Drawer d = new Textures.Drawer();
        private void OnGUI()
        {
            TryInit();

            #region Main setup
            //var debugFlags = DebugDisplayRegistry.DebugFlags;
            var enumType = (Type)null;//debugFlags.maskType;
            if (enumType == null || !enumType.IsEnum)
                return;

            enumInfos.Init(enumType);
            activeFlags.Count = enumInfos.Count;
            expandedFlags.Count = enumInfos.Count;

            if (styleSetup == null)
            {
                styleSetup = new GUISetup();
            }
            #endregion Main setup

            #region Draw setup
            var bgMargin = 2;
            var lineMargin = 4;
            #endregion Draw setup

            var maskHasChanged = false;
            var itemCount = 0;
            for (int e = 0; e < enumInfos.Count; e++)
            {
                var value = enumInfos[e].value;
                //if (IsExpanded(value, ref debugFlags))
                //    itemCount += 1;
            }

            var titleRect = GUIDraw.BackgroundAuto(2, lineMargin, bgMargin, styleSetup.background);
            {
                titleRect = titleRect.TruncateX(titleRect.height)
                                     .TruncateX(-titleRect.height)
                                     .Inflate(-titleRect.height / 8);
                GUIDraw.Background(titleRect, 2, styleSetup.itemBG[0]);
                titleRect = titleRect.TruncateX(titleRect.height / 2);
                GUI.Label(titleRect, enumInfos.Type.Name, styleSetup.titleText);
            }

            var winRect = GUIDraw.BackgroundAuto(itemCount, lineMargin, bgMargin, styleSetup.background);
            {
                var lineRect = winRect;
                lineRect.height /= itemCount;
                for (int e = 0; e < enumInfos.Count; e++)
                {
                    var value = enumInfos[e].value;
                    var parentCount = 0;//debugFlags.CountParent(value);
                    var hasChildren = 0;//debugFlags.HasChildren(value);
                    //if (IsExpanded(value, ref debugFlags))
                    {
                        var line = lineRect;
                        var lockSize = 0.8f;

                        {
                            var lockRect = line.nh(line.height * (1.0f + lockSize));
                            GUIDraw.Background(lockRect, 0, styleSetup.itemBG[0]);
                            lockRect = lockRect.TruncateX(lockRect.height);
                            GUIDraw.Background(lockRect.Inflate(-2), 0, styleSetup.actives[0]);
                        }

                        //On-Off square
                        var activeRect = GUIDraw.Square(GUIDraw.Square(ref line, 2), 0, styleSetup.actives[activeFlags[e] ? 1 : 0]);
                        if (Event.current.type == EventType.MouseUp && activeRect.Contains(Event.current.mousePosition))
                        {
                            maskHasChanged = true;
                            activeFlags[e] = !activeFlags[e];
                            Repaint();
                        }

                        //Parent offsets
                        line = line.TruncateX(line.height * ((float)parentCount + lockSize));

                        line = GUIDraw.Background(line, 2, styleSetup.itemBG[0]);

                        if (Event.current.type == EventType.MouseUp && line.Contains(Event.current.mousePosition))
                        {
                            expandedFlags[e] = !expandedFlags[e];
                            Repaint();
                        }

                        //Does this option has children 
                        //GUIDraw.Square(ref line, 0, hasChildren ? styleSetup.actives[0] : null);

                        line = line.TruncateX(line.height / 2);
                        GUI.Label(line, enumInfos[e].name, styleSetup.itemText);

                        lineRect = lineRect.NextLine();
                    }

                }
            }

            //todo var manager = !EditorApplication.isPlaying ? null : TickableRegistry.GetManager<DebugDisplayManager>();
            //todo if (manager != null)
            //todo {
            //todo     if (maskHasChanged)
            //todo     {
            //todo         for (int e = 0; e < enumInfos.Count; e++)
            //todo         {
            //todo             debugFlags.SetStatus(enumInfos[e].value, activeFlags[e]);
            //todo         }
            //todo         DebugDisplayManager.DebugFlags = debugFlags;
            //todo         manager.Build();
            //todo     }
            //todo }
            return;


            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                /*
                #region Draw setup
                var margin2 = Vector2.one * 2;
                var margin4 = Vector2.one * 4;
                var windowRect = EditorGUILayout.GetControlRect(GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * enumInfos.Count + margin4.y * 2));
                var lineRect = windowRect;
                {
                    lineRect.x += margin4.x;
                    lineRect.width -= margin4.x * 2;
                    lineRect.height = EditorGUIUtility.singleLineHeight;
                    lineRect.y = windowRect.height - (lineRect.height + margin4.y);
                }
                var boxRect = lineRect;
                {
                    boxRect.width = lineRect.height - margin2.y * 2;
                    boxRect.height = lineRect.height - margin2.x * 2;
                    boxRect.x += margin2.x;
                    boxRect.y += margin2.y;
                }
                var textRect = lineRect;
                {
                    textRect.x += boxRect.width + margin2.x * 2;
                    textRect.width -= boxRect.width + margin2.x * 3;
                    textRect.height = EditorGUIUtility.singleLineHeight;
                }
                #endregion Draw setup

                #region Draw background
                GUI.Box(windowRect, GUIContent.none, styleSetup.background);
                #endregion Draw background

                #region Draw lines

                for (int i = enumInfos.Count - 1; i >= 0; i--)
                {
                    var indent = manager != null ? manager.CountParents(i) : 0;
                    var oldX = lineRect.x;
                    var oldWidth = lineRect.width;
                    {
                        lineRect.x += indent * lineRect.height;
                        lineRect.width -= indent * lineRect.height;
                        boxRect.x += indent * lineRect.height;
                        textRect.x += indent * lineRect.height;

                        GUI.Box(lineRect, GUIContent.none, styleSetup.m_item_background[Mathf.Min(indent, styleSetup.m_item_background.Count)]);
                        GUI.Box(boxRect, GUIContent.none, styleSetup.m_active[i % 2]);
                        GUI.Label(textRect, enumInfos[i].name, styleSetup.m_item_text);
                    }
                    lineRect.x = oldX;
                    lineRect.width = oldWidth;
                    lineRect.y -= lineRect.height;
                    boxRect.x = oldX + margin2.x;
                    boxRect.y -= lineRect.height;
                    textRect.x = oldX + boxRect.width + margin2.x * 2;
                    textRect.y -= lineRect.height;
                }

                #endregion Draw lines
                */
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
        #endregion Unity Defaults

        ///---------------------------------------------------------------------
        #region Behaviour
        #endregion Behaviour
    }
}
#endif //PRATEEK_DEBUG

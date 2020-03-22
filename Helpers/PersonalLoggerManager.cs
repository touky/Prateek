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
//
//-----------------------------------------------------------------------------
#region C# Prateek Namespaces

//Auto activate some of the prateek defines
#if UNITY_EDITOR

#if !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //!PRATEEK_DEBUG

#endif //UNITY_EDITOR && !PRATEEK_DEBUG

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_IFDEF-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
#if PRATEEK_DEBUG
namespace Prateek.Debug
{
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Extensions;
    using UnityEditor;
    using UnityEngine;
    using static Prateek.ShaderTo.CSharp;

    //-------------------------------------------------------------------------
    public class PersonalLoggerManager
    {
        //---------------------------------------------------------------------
        #region Declarations
        public class GUISetup
        {
            public Font Labelfont = null;
            public GUIStyle BGBoxStyle = null;
            public GUIStyle LabelStyle = null;
            public GUIStyle ScrollViewStyle = null;
            public GUIStyle ToolbarActive = null;
            public GUIStyle ToolbarInactive = null;
            public GUIStyle OffsetLeftActive = null;
            public GUIStyle OffsetLeftInactive = null;
            public GUIStyle OffsetRightActive = null;
            public GUIStyle OffsetRightInactive = null;
            public Vector2 CharacterRefSize = Vector2.one;
            public bool DefaultWindowRectIsValid = false;
            public Rect DefaultWindowRect = new Rect(new Vector2(10, 10), Vector2.zero);
            public Rect WindowRect = new Rect(new Vector2(10, 10), Vector2.zero);
            public Vector2? LastMousePosition = null;
            public int ActiveBox = 0;
            public int BoxOffset = 0;
            public bool HasAnyActiveItem = false;

            public void Init()
            {
                //TODO BHU ---------------------
                var settings = new Helpers.PersonalLogger.GUISettings(); //GlobalSettings.OcpBlackBoxDebugSettings;

#if UNITY_EDITOR
                if (!DefaultWindowRectIsValid)
                {
                    DefaultWindowRectIsValid = true;
                    //Get editor pref stuff
                    DefaultWindowRect = new Rect(new Vector2(EditorPrefs.GetFloat(settings.WindowRectPrefNameX, DefaultWindowRect.x),
                                                             EditorPrefs.GetFloat(settings.WindowRectPrefNameY, DefaultWindowRect.y)), Vector2.zero);

                    //No resize for now
                    //WindowRect.size = new Vector2(EditorPrefs.GetFloat(settings.WindowRectPrefNameW, windowDefaultRect.width),
                    //                EditorPrefs.GetFloat(settings.WindowRectPrefNameH, windowDefaultRect.height));

                }
#endif //UNITY_EDITOR

                if (BGBoxStyle == null)
                {
                    BGBoxStyle = new GUIStyle(GUI.skin.box);
                    BGBoxStyle.normal.background = Helpers.Textures.Make(new Color(0, 0, 0, 0.8f), Color.black);
                    BGBoxStyle.contentOffset = Vector2.zero;
                    BGBoxStyle.border = new RectOffset(1, 1, 1, 1);
                }

                if (Labelfont == null)
                {
                    Labelfont = settings.Labelfont != null ? settings.Labelfont : Font.CreateDynamicFontFromOSFont("Consolas", GUI.skin.font.fontSize);
                }

                if (LabelStyle == null)
                {
                    LabelStyle = settings.LabelStyle != null ? settings.LabelStyle : new GUIStyle(GUI.skin.label);
                    LabelStyle.richText = true;
                    LabelStyle.font = Labelfont;
                    LabelStyle.normal.textColor = Color.white;
                    LabelStyle.alignment = TextAnchor.LowerLeft;
                    CharacterRefSize = LabelStyle.CalcSize(new GUIContent("A"));
                }

                if (ScrollViewStyle == null)
                {
                    ScrollViewStyle = settings.ScrollViewStyle != null ? settings.ScrollViewStyle : new GUIStyle(GUI.skin.scrollView);
                }

                if (ToolbarActive == null)
                {
                    //GUI.skin.customStyles.
                    ToolbarActive = Array.Find(GUI.skin.customStyles, (x) => { if (x.name == "flow node 0") return true; return false; });
#if UNITY_EDITOR
                    ToolbarActive = ToolbarActive == null ? EditorStyles.toolbarButton : ToolbarActive;
                    ToolbarActive = new GUIStyle(ToolbarActive);
#else //UNITY_EDITOR
                    ToolbarActive = new GUIStyle(GUI.skin.box);
                    ToolbarActive.normal.background = Utils.MakeTex3x3(new Color(0.4f, 0.4f, 0.4f, 0.9f), new Color(0.8f, 0.8f, 0.8f, 0.9f));
                    ToolbarActive.contentOffset = Vector2.zero;
                    ToolbarActive.border = new RectOffset(1, 1, 1, 1);
#endif //UNITY_EDITOR
                    ToolbarActive.alignment = TextAnchor.MiddleRight;
                }

                if (ToolbarInactive == null)
                {
                    ToolbarInactive = Array.Find(GUI.skin.customStyles, (x) => { if (x.name == "flow background") return true; return false; });
#if UNITY_EDITOR
                    ToolbarInactive = ToolbarInactive == null ? EditorStyles.toolbarButton : ToolbarInactive;
                    ToolbarInactive = new GUIStyle(ToolbarInactive);
#else //UNITY_EDITOR
                    ToolbarInactive = new GUIStyle(GUI.skin.box);
                    ToolbarInactive.normal.background = Utils.MakeTex3x3(new Color(0.0f, 0.0f, 0.0f, 0.9f), new Color(0.4f, 0.4f, 0.4f, 0.9f));
                    ToolbarInactive.contentOffset = Vector2.zero;
                    ToolbarInactive.border = new RectOffset(1, 1, 1, 1);
#endif //UNITY_EDITOR
                    ToolbarInactive.alignment = TextAnchor.MiddleRight;
                }

                if (OffsetLeftActive == null)
                {
#if UNITY_EDITOR
                    OffsetLeftActive = EditorStyles.miniButtonLeft;
                    OffsetLeftActive = new GUIStyle(OffsetLeftActive);
#else //UNITY_EDITOR
                    OffsetLeftActive = new GUIStyle(GUI.skin.button);
#endif //UNITY_EDITOR
                }

                if (OffsetLeftInactive == null)
                {
#if UNITY_EDITOR
                    OffsetLeftInactive = EditorStyles.miniButtonLeft;
                    OffsetLeftInactive = new GUIStyle(OffsetLeftInactive);
#else //UNITY_EDITOR
                    OffsetLeftInactive = new GUIStyle(GUI.skin.button);
#endif //UNITY_EDITOR
                }

                if (OffsetRightActive == null)
                {
#if UNITY_EDITOR
                    OffsetRightActive = EditorStyles.miniButtonRight;
                    OffsetRightActive = new GUIStyle(OffsetRightActive);
#else //UNITY_EDITOR
                    OffsetRightActive = new GUIStyle(GUI.skin.button);
#endif //UNITY_EDITOR
                }

                if (OffsetRightInactive == null)
                {
#if UNITY_EDITOR
                    OffsetRightInactive = EditorStyles.miniButtonRight;
                    OffsetRightInactive = new GUIStyle(OffsetRightInactive);
#else //UNITY_EDITOR
                    OffsetRightInactive = new GUIStyle(GUI.skin.button);
#endif //UNITY_EDITOR
                }
            }
        }

        //---------------------------------------------------------------------
        public class RegisteredLogger
        {
            public Helpers.PersonalLogger logger;
            public Prateek.Debug.Flag.Overridable show = false;
            public Vector2 scroll = new Vector2(0, 0);
            public float scroll_inner_width = 600f;
        }
        #endregion //Declarations

        //---------------------------------------------------------------------
        #region Fields
        private PersonalLoggerManager.GUISetup m_gui_setup = new PersonalLoggerManager.GUISetup();
        private List<PersonalLoggerManager.RegisteredLogger> m_loggers = new List<PersonalLoggerManager.RegisteredLogger>();
        #endregion //Fields

        public PersonalLoggerManager()
        {
        }

        //---------------------------------------------------------------------
        #region Registering
        public void Register(Helpers.PersonalLogger box)
        {
            if (box == null)
                return;

            for (int i = 0; i < m_loggers.Count; i++)
            {
                if (m_loggers[i].logger == box)
                    return;
            }

            m_loggers.Add(new RegisteredLogger() { logger = box, show = false });
        }

        //---------------------------------------------------------------------
        public void Unregister(Helpers.PersonalLogger logger)
        {
            if (logger == null)
                return;

            for (int i = 0; i < m_loggers.Count; i++)
            {
                if (m_loggers[i].logger == logger)
                {
                    m_loggers.RemoveAt(i);
                    return;
                }
            }
        }
        #endregion Registering

        //---------------------------------------------------------------------
        #region GUI
        public void DisplayDebug()
        {
            var camera = UnityEngine.Camera.current;
            var style = m_gui_setup;
            if (style != null && style.HasAnyActiveItem)
            {
                for (int i = 0; i < m_loggers.Count; i++)
                {
                    if (i == style.ActiveBox)
                    {
                        var box = m_loggers[i];
                        var robotPos = camera.WorldToScreenPoint(box.logger.transform.position);
                        {
                            robotPos.y = Screen.height - robotPos.y;
                            var guiPos = new Vector2(Mathf.Clamp(robotPos.x, style.WindowRect.x, style.WindowRect.x + style.WindowRect.width),
                                                     Mathf.Clamp(robotPos.y, style.WindowRect.y, style.WindowRect.y + style.WindowRect.height));
                            guiPos.y = Screen.height - guiPos.y;
                            var worldGuiPos = camera.ScreenToWorldPoint(guiPos.xyn(1f));
                            DebugDraw.Line(DebugDraw.DebugPlace.AToB(worldGuiPos, box.logger.transform.position), new DebugDraw.DebugStyle(DebugDraw.Space.World, Color.white));
                        }
                        break;
                    }
                }
            }
        }

        //---------------------------------------------------------------------
        public void DisplayGUI()
        {
            if (m_gui_setup != null)
            {
                m_gui_setup.Init();
            }
            //TODO BHU ---------------------
            var settings = new Helpers.PersonalLogger.GUISettings(); //GlobalSettings.OcpBlackBoxDebugSettings;
            var style = m_gui_setup;

            style.HasAnyActiveItem = false;
            if (style.ActiveBox >= 0 && style.ActiveBox < m_loggers.Count)
            {
                if (m_loggers[style.ActiveBox].logger == null || !m_loggers[style.ActiveBox].show.CanUse)
                {
                    style.ActiveBox = -1;
                }
            }

            for (int i = 0; i < m_loggers.Count; i++)
            {
                if (m_loggers[i].logger != null && m_loggers[i].show.CanUse)
                {
                    if (style.ActiveBox < 0)
                    {
                        style.ActiveBox = i;
                    }
                    style.HasAnyActiveItem = true;
                    break;
                }
            }

            if (!style.HasAnyActiveItem)
                return;

            var oldGUIColor = GUI.color;

            var minNameSize = 50f;

            var logSize = new Vector2(settings.WindowCharacterCount.x * style.CharacterRefSize.x, settings.WindowCharacterCount.y * style.CharacterRefSize.y);
            var headerSize = new Vector2(logSize.x, style.ToolbarActive.lineHeight * 2f);
            var buttonsSize = new Vector2(style.ToolbarActive.lineHeight * 4, style.ToolbarActive.lineHeight * 1.5f);
            var windowDefaultRect = new Rect(style.WindowRect.position, logSize + Vector2.up * headerSize.y);

            var windowPosition = style.DefaultWindowRect.position;
            style.WindowRect.position = style.DefaultWindowRect.position;
            style.WindowRect.size = new Vector2(windowDefaultRect.width, windowDefaultRect.height);

            //Resize to fit the screen
            style.WindowRect.size = (min(style.WindowRect.position + style.WindowRect.size, new Vector2(Screen.width, Screen.height)) - style.WindowRect.position);
            style.WindowRect.size = min(style.WindowRect.size, style.WindowRect.size - (max(style.WindowRect.position, 0) - style.WindowRect.position));
            style.WindowRect.position = max(style.WindowRect.position, 0);

            //Build rects
            var buttonsRect = new Rect(style.WindowRect.position + max(Vector2.right * (style.WindowRect.width - buttonsSize.x), 0), buttonsSize);
            var headerRect = new Rect(style.WindowRect.position, max(vec2(style.WindowRect.width - (buttonsRect.width + 10), headerSize.y), 0));
            var scrollViewRect = new Rect(style.WindowRect.position + Vector2.up * headerRect.height, max(style.WindowRect.size - Vector2.up * headerSize.y, 0));

            GUI.Box(style.WindowRect, GUIContent.none, style.BGBoxStyle);

            var newActiveBox = style.ActiveBox;
            var content = new GUIContent();
            var cursorRect = new Rect(style.WindowRect.position, Vector2.up * headerRect.height);
            var hasItemsOnTheRight = false;
            for (int i = 0; i < m_loggers.Count; i++)
            {
                if (m_loggers[i].logger != null && m_loggers[i].show.CanUse)
                {
                    var isToggled = style.ActiveBox == i;
                    if (headerRect.width > minNameSize)
                    {
                        if (i >= style.BoxOffset)
                        {
                            //Draw selection Tabs
                            content.text = m_loggers[i].logger.name;
                            content.tooltip = m_loggers[i].logger.name;
                            var nameSize = min(max(style.ToolbarActive.CalcSize(content), minNameSize), headerRect.width).x;
                            var toggleRect = new Rect(cursorRect.position, new Vector2(nameSize, cursorRect.height));

                            if (GUI.Toggle(toggleRect, isToggled, content, isToggled ? style.ToolbarActive : style.ToolbarInactive) != isToggled)
                            {
                                newActiveBox = i;
                            }

                            var offset = min(Vector2.right * toggleRect.width, headerRect.width);
                            cursorRect.position += offset;
                            headerRect.size -= offset;
                            headerRect.position += offset;
                        }
                    }
                    else
                    {
                        hasItemsOnTheRight = true;
                    }

                    //Show the black box log
                    if (isToggled)
                    {
                        m_loggers[i].logger.Display(scrollViewRect, m_loggers[i], style);
                    }
                }
            }

            //Prev-Next button logic
            {
                buttonsRect.size = new Vector2(buttonsRect.width / 2f, buttonsRect.height);
                var hasPrev = style.BoxOffset > 0;
                var goPrev = GUI.Toggle(buttonsRect, hasPrev, "<", hasPrev ? style.OffsetLeftActive : style.OffsetLeftInactive);
                if (hasPrev && hasPrev != goPrev)
                {
                    style.BoxOffset = Mathf.Max(0, style.BoxOffset - 1);
                }

                buttonsRect.position += buttonsRect.size.xyn(0).xzy().xy();
                var goNext = GUI.Toggle(buttonsRect, hasItemsOnTheRight, ">", hasItemsOnTheRight ? style.OffsetRightActive : style.OffsetRightInactive);
                if (hasItemsOnTheRight && hasItemsOnTheRight != goNext)
                {
                    style.BoxOffset += 1;
                }
            }

            //Drag logic: don't use the style.windowRect.position in this
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    {
                        if (scrollViewRect.Contains(Event.current.mousePosition))
                        {
                            if (style.LastMousePosition == null)
                            {
                                style.LastMousePosition = Event.current.mousePosition;
                            }
                        }
                        break;
                    }
                case EventType.MouseDrag:
                    {
                        if (style.LastMousePosition != null)
                        {
                            windowPosition += Event.current.mousePosition - style.LastMousePosition.Value;
                            style.LastMousePosition = Event.current.mousePosition;
                        }
                        break;
                    }
                case EventType.MouseUp:
                case EventType.DragExited:
                    {
                        style.LastMousePosition = null;
                        break;
                    }
            }

            windowPosition = max(min(windowPosition, new Vector2(Screen.width, Screen.height) - Vector2.one * headerRect.height * 4f), -(windowDefaultRect.size - Vector2.one * headerRect.height * 2f));
            style.ActiveBox = newActiveBox;

            //Set position after moving the mouse
            if (windowPosition != style.DefaultWindowRect.position)
            {
                style.DefaultWindowRect.position = windowPosition;
#if UNITY_EDITOR
                EditorPrefs.SetFloat(settings.WindowRectPrefNameX, style.DefaultWindowRect.position.x);
                EditorPrefs.SetFloat(settings.WindowRectPrefNameY, style.DefaultWindowRect.position.y);
#endif //UNITY_EDITOR
            }

            GUI.color = oldGUIColor;
        }
        #endregion GUI
    }
}

namespace Prateek.Helpers
{
    using System;
    using Debug;
    using Extensions;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public partial class PersonalLogger : MonoBehaviour
    {
        //---------------------------------------------------------------------
        #region Declarations
        [Serializable]
        public class GUISettings
        {
            public string WindowRectPrefNameX = "PersonalLogger.GUISettings.WindowRect.x";
            public string WindowRectPrefNameY = "PersonalLogger.GUISettings.WindowRect.y";
            public string WindowRectPrefNameW = "PersonalLogger.GUISettings.WindowRect.w";
            public string WindowRectPrefNameH = "PersonalLogger.GUISettings.WindowRect.h";

            public string LabelfontName = "Consolas";
            public Font Labelfont = null;
            public GUIStyle LabelStyle = null;
            public GUIStyle ScrollViewStyle = null;
            public Vector2 WindowCharacterCount = new Vector2(80, 25);
            public string LogBaseSeparatorText = "    ";
            public string TaskStartSeparatorText = "[-- ";
            public string TaskEndSeparatorText = "==] ";
            public string LogNameSeparatorText = ": ";
            public string OwnerNameSeparatorText = "/";
            public string LogTimeText = "{2:D2}:{1:D2}:{0:D3}> ";
            public string LogTimeHourText = "{3:D2}:";
            public Color[] LogColors = new Color[(int)PersonalLogger.LogType.MAX]
                    {
                    /* Nothing******/ Color.black,
                    /* Error *******/ Color.red,
                    /* Warning *****/ Color.yellow,

                    /* Minor *******/ new Color(0.5f, 0.5f, 0.5f),
                    /* Medium ******/ new Color(0.75f, 0.75f, 0.75f),
                    /* Major *******/ Color.white,
                    /* Extreme *****/ Color.cyan,

                    /* TaskStart ***/ new Color(125f / 255f, 161f / 255f, 245f / 255f),
                    /* TaskEnd *****/ new Color(153f / 255f, 165f / 255f, 194f / 255f),
                    /* TaskEndFail */ new Color(235f / 255f, 183f / 255f, 63f / 255f),
                    };
        };
        #endregion Declarations

        //---------------------------------------------------------------------
        public void Display(Rect rect, PersonalLoggerManager.RegisteredLogger logger, PersonalLoggerManager.GUISetup gui_setup)
        {
            var oldGUIColor = GUI.color;
            //TODO BHU ---------------------
            var settings = new PersonalLogger.GUISettings();

            //Variables
            var logHistory = m_log_history;

            var scrollView = new Rect(Vector2.zero, new Vector2(logger.scroll_inner_width, gui_setup.CharacterRefSize.y * logHistory.Count));

            GUI.color = oldGUIColor;
            var oldscrollViewStyle = GUI.skin.scrollView;
            GUI.skin.scrollView = gui_setup.ScrollViewStyle;

            var scrollPos = new Vector2(logger.scroll.x, (scrollView.height - rect.height) - logger.scroll.y);
            scrollPos = GUI.BeginScrollView(rect, scrollPos, scrollView);
            logger.scroll = new Vector2(scrollPos.x, (scrollView.height - rect.height) - scrollPos.y);
            {
                var startPos = Vector2.zero;
                var currentPos = startPos;

                LogData lastLog = new LogData();
                var logContent = new GUIContent();
                foreach (var log in logHistory)
                {
                    var logText = string.Empty;
                    var usedColor = settings.LogColors[(int)log.Type];
                    var minorColor = settings.LogColors[(int)PersonalLogger.LogType.Minor];

                    //DATA -----------------------------------------------
                    //Build log time
                    var timeStr = Helpers.Format.Time(log.Timestamp, settings.LogTimeText, settings.LogTimeHourText);

                    //Build Task start/end
                    var taskStr = settings.LogBaseSeparatorText;
                    switch (log.Type)
                    {
                        case PersonalLogger.LogType.BlockStart:
                            taskStr = settings.TaskStartSeparatorText;
                            break;
                        case PersonalLogger.LogType.BlockEndSuccess:
                        case PersonalLogger.LogType.BlockEndFail:
                            taskStr = settings.TaskEndSeparatorText;
                            break;
                    }

                    //Build owner name
                    var ownerCount = log.Owners.Count;
                    var objName = string.Empty;
                    bool atLeastOneNewOwner = false;
                    for (int i = 0; i < ownerCount; i++)
                    {
                        var owner = log.Owners[i];
                        if (owner != null)
                        {
                            var newObjName = string.Empty;
                            if (i > 0)
                            {
                                newObjName += settings.OwnerNameSeparatorText;
                            }

                            //TODO BHU
                            /*
                            var ocp = owner as OcpBase;
                            var ctlr = owner as BaseCharacterController;
                            var block = owner as Programming.Block;

                            if (ocp != null) { newObjName += ocp.Name; }
                            else if (ctlr != null) { newObjName += ctlr.Name; }
                            else if (block != null) { newObjName += block.GetBlockInfo().name.Replace('\n', ' '); }
                            else*/
                            { newObjName += owner.GetType().Name; }

                            if (lastLog.Owners[i] == owner)
                            {
                                newObjName = Helpers.Format.Color(newObjName, minorColor);
                                atLeastOneNewOwner = true;
                            }

                            objName += newObjName;
                        }

                        if (i + 1 == ownerCount)
                        {
                            objName += atLeastOneNewOwner ? Helpers.Format.Color(settings.LogNameSeparatorText, minorColor) : settings.LogNameSeparatorText;
                        }
                    }
                    lastLog.Owners.Copy(log.Owners);

                    //Build log text
                    logText = timeStr + taskStr + objName + Helpers.Format.Color(log.Log.Build(), usedColor);

                    //Draw log label
                    logContent.text = logText;
                    var size = gui_setup.LabelStyle.CalcSize(logContent);
                    GUI.Label(new Rect(currentPos, size), logText, gui_setup.LabelStyle);

                    //Update max inner width
                    logger.scroll_inner_width = Mathf.Max(logger.scroll_inner_width, size.x);

                    //Advance to next label
                    currentPos.y += size.y;
                }
            }
            GUI.EndScrollView();

            GUI.skin.scrollView = oldscrollViewStyle;
            GUI.color = oldGUIColor;
        }
    }
}
#endif //PRATEEK_DEBUG

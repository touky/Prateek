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
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public class GUIStyles : SharedStorage
    {
        //---------------------------------------------------------------------
        public struct Setup
        {
            public GUIStyle source;
            public Vector2 content_offset;
            public RectOffset border;
            public int font_size;
            public Color normal_text_Color;

            public override string ToString()
            {
                return string.Format("{0}_{1:F2}_{2:F2}_{3:F2}_{4:F2}_{5:F2}_{6:F2}_{7}_{8}",
                                    source.name,
                                    content_offset.x, content_offset.y,
                                    border.bottom, border.top, border.left, border.right,
                                    font_size, Format.ToRichText(normal_text_Color));
            }
        }
        private Setup m_setup;

        //---------------------------------------------------------------------
        private static GUIStyles m_instance = null;
        private static GUIStyles Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new GUIStyles();
                return m_instance;
            }
        }

        //---------------------------------------------------------------------
        public static GUIStyle Get(GUIStyle source, Vector2 content_offset,
                                   RectOffset border, int font_size,
                                   Color normal_text_Color)
        {
            return Get(string.Empty, source, content_offset, border, font_size, normal_text_Color);
        }

        //---------------------------------------------------------------------
        public static GUIStyle Get(string name,
                                   GUIStyle source, Vector2 content_offset,
                                   RectOffset border, int font_size,
                                   Color normal_text_Color)
        {
            Instance.m_setup = new Setup()
            {
                source = source,
                content_offset = content_offset,
                border = border,
                font_size = font_size,
                normal_text_Color = normal_text_Color
            };
            return Instance.GetInstance(name + Instance.m_setup.ToString()) as GUIStyle;
        }

        //---------------------------------------------------------------------
        protected override object CreateInstance(string key)
        {
            var style = new GUIStyle(Instance.m_setup.source);
            style.name = key;
            style.contentOffset = Instance.m_setup.content_offset;
            style.border = Instance.m_setup.border;
            style.fontSize = Instance.m_setup.font_size;
            style.normal.textColor = Instance.m_setup.normal_text_Color;
            return style;
        }
    }
}


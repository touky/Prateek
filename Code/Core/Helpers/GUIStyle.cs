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
namespace Prateek.Core.Code.Helpers
{
    using UnityEngine;

    ///-------------------------------------------------------------------------
    public class GUIStyles : SharedStorage
    {
        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public static GUIStyle Get(GUIStyle source, Vector2 content_offset,
                                   RectOffset border, int font_size,
                                   Color normal_text_Color)
        {
            return Get(string.Empty, source, content_offset, border, font_size, normal_text_Color);
        }

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
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


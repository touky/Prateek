// -BEGIN_PRATEEK_COPYRIGHT-
//
//  Prateek, a library that is "bien pratique"
//  Header last update date: 24/03/2019
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
    public class Textures : SharedStorage
    {
        //---------------------------------------------------------------------
        public struct Setup
        {
            public Rect inner_rect;
            public Color content;
            public Color border;

            public override string ToString()
            {
                return string.Format("{0:F2}_{1:F2}_{2:F2}_{3:F2}_{4}_{5}",
                                    inner_rect.x, inner_rect.y, inner_rect.width, inner_rect.height,
                                    Format.ToRichText(content),
                                    Format.ToRichText(border));
            }
        }
        private Setup m_setup;

        //---------------------------------------------------------------------
        private static Textures m_instance = null;
        private static Textures Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Textures();
                return m_instance;
            }
        }

        //---------------------------------------------------------------------
        public static Texture2D Make(Color content)
        {
            return Make(new Rect(1, 1, 1, 1), content, content);
        }

        //---------------------------------------------------------------------
        public static Texture2D Make(Color content, Color border)
        {
            return Make(new Rect(1, 1, 1, 1), content, border);
        }

        //---------------------------------------------------------------------
        public static Texture2D Make(Rect inner_rect, Color content, Color border)
        {
            Instance.m_setup = new Setup()
            {
                inner_rect = inner_rect,
                content = content,
                border = border
            };
            return Instance.GetInstance(Instance.m_setup.ToString()) as Texture2D;
        }

        //---------------------------------------------------------------------
        protected override object CreateInstance(string key)
        {
            Vector2 size = new Vector2((int)(m_setup.inner_rect.x * 2 + m_setup.inner_rect.width),
                                       (int)(m_setup.inner_rect.y * 2 + m_setup.inner_rect.height));
            Color[] pix = new Color[(int)(size.Area())];

            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = m_setup.inner_rect.Contains(Vectors.FromIndex(i, size)) ? m_setup.content : m_setup.border;
            }

            Texture2D result = new Texture2D((int)size.x, (int)size.y);
            result.name = key;
            result.SetPixels(pix);
            result.Apply();
            result.filterMode = FilterMode.Point;
            return result;
        }
    }
}
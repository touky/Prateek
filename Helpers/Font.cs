//
//  Prateek, a library that is "bien pratique"
//
//  Copyright © 2017—2018 Benjamin “Touky” Huet <huet.benjamin@gmail.com>
//
//  Prateek is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

#region Namespaces
#if UNITY_EDITOR && !PRATEEK_DEBUG
#define PRATEEK_DEBUG
#endif //UNITY_EDITOR && !PRATEEK_DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif //UNITY_EDITOR

#if UNITY_PROFILING
using UnityEngine.Profiling;
#endif //UNITY_PROFILING

using Prateek;
using Prateek.Base;
using Prateek.Extensions;
using Prateek.Attributes;

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Helpers
{
    //-------------------------------------------------------------------------
    public class Fonts : SharedStorage
    {
        //---------------------------------------------------------------------
        public struct Setup
        {
            public string name;
            public int size;

            public override string ToString()
            {
                return string.Format("{0}_{1}", name, size);
            }
        }
        private Setup m_setup;

        //---------------------------------------------------------------------
        private static Fonts m_instance = null;
        private static Fonts Instance
        {
            get
            {
                if (m_instance == null)
                    m_instance = new Fonts();
                return m_instance;
            }
        }

        //---------------------------------------------------------------------
        public static Font Get(string name, int size)
        {
            Instance.m_setup = new Setup() { name = name, size = size };
            return Instance.GetInstance(Instance.m_setup.ToString()) as Font;
        }

        //---------------------------------------------------------------------
        protected override object CreateInstance(string key)
        {
            var font = Font.CreateDynamicFontFromOSFont(m_setup.name, m_setup.size);
            font.name = key;
            return font;
        }
    }
}


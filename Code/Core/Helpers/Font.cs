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
    public class Fonts : SharedStorage
    {
        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
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

        ///---------------------------------------------------------------------
        public static Font Get(string name, int size)
        {
            Instance.m_setup = new Setup() { name = name, size = size };
            return Instance.GetInstance(Instance.m_setup.ToString()) as Font;
        }

        ///---------------------------------------------------------------------
        protected override object CreateInstance(string key)
        {
            var font = Font.CreateDynamicFontFromOSFont(m_setup.name, m_setup.size);
            font.name = key;
            return font;
        }
    }
}


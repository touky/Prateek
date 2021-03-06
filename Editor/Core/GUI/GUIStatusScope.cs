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

namespace Prateek.Editor.Core.GUI
{
    using UnityEngine;

    public class GUIStatusScope : GUI.Scope
    {
        ///---------------------------------------------------------------------
        #region Fields
        private bool enable;
        private Color color;
        #endregion Fields

        ///---------------------------------------------------------------------
        #region Properties
        public bool Enable { set { GUI.enabled = value; } }
        public Color Color { set { GUI.color = value; } }
        #endregion Properties

        ///---------------------------------------------------------------------
        #region Scope
        public GUIStatusScope(bool enable) : this(enable, GUI.color) { }
        public GUIStatusScope(Color color) : this(true, color) { }
        public GUIStatusScope(bool enable, Color color) : base()
        {
            this.enable = GUI.enabled;
            this.color = GUI.color;

            GUI.enabled = GUI.enabled && enable;
            GUI.color = color;
        }

        ///---------------------------------------------------------------------
        public void Reset()
        {
            GUI.enabled = enable;
            GUI.color = color;
        }

        ///---------------------------------------------------------------------
        protected override void CloseScope()
        {
            Reset();
        }
        #endregion Scope
    }
}
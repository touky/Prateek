// -BEGIN_PRATEEK_COPYRIGHT-// -BEGIN_PRATEEK_COPYRIGHT-
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
// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-// -END_PRATEEK_COPYRIGHT-

// -BEGIN_PRATEEK_CSHARP_NAMESPACE-// -BEGIN_PRATEEK_CSHARP_NAMESPACE-
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
// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Editors
{
    using UnityEditor;
    using UnityEngine;

    //-------------------------------------------------------------------------
    public static class GUIs
    {
        //---------------------------------------------------------------------
        #region Foldout

        //---------------------------------------------------------------------
        #region Overload
        public static bool Foldout(Rect rect, string text, string key, bool toggleOnLabelClick = true)
        {
            return Foldout(false, rect, new GUIContent(text), key, toggleOnLabelClick);
        }
        public static bool Foldout(Rect rect, GUIContent content, string key, bool toggleOnLabelClick = true)
        {
            return Foldout(false, rect, content, key, toggleOnLabelClick);
        }
        public static bool Foldout(string text, string key, bool toggleOnLabelClick = true)
        {
            return Foldout(true, new Rect(), new GUIContent(text), key, toggleOnLabelClick);
        }
        public static bool Foldout(GUIContent content, string key, bool toggleOnLabelClick = true)
        {
            return Foldout(true, new Rect(), content, key, toggleOnLabelClick);
        }
        #endregion Overload

        //---------------------------------------------------------------------
        private static bool Foldout(bool isLayout, Rect rect, GUIContent content, string key, bool toggleOnLabelClick = true)
        {
            var foldoutActive = Prateek.Editors.Prefs.Get(key, false);
            EditorGUI.BeginChangeCheck();
            var tempActive = foldoutActive.Value;
            if (!isLayout)
            {
                tempActive = EditorGUI.Foldout(rect, foldoutActive.Value, content, toggleOnLabelClick);
            }
            else
            {
                tempActive = EditorGUILayout.Foldout(foldoutActive.Value, content, toggleOnLabelClick);
            }

            if (EditorGUI.EndChangeCheck())
            {
                foldoutActive.Value = tempActive;
            }

            return tempActive;
        }
        #endregion Foldout

        //---------------------------------------------------------------------
        public class StatusScope : GUI.Scope
        {
            //-----------------------------------------------------------------
            #region Fields
            private bool enable;
            private Color color;
            #endregion Fields

            //-----------------------------------------------------------------
            #region Properties
            public bool Enable { set { GUI.enabled = value; } }
            public Color Color { set { GUI.color = value; } }
            #endregion Properties

            //-----------------------------------------------------------------
            #region Scope
            public StatusScope(bool enable) : this(enable, GUI.color) { }
            public StatusScope(Color color) : this(true, color) { }
            public StatusScope(bool enable, Color color) : base()
            {
                this.enable = GUI.enabled;
                this.color = GUI.color;

                GUI.enabled = GUI.enabled && enable;
                GUI.color = color;
            }

            //-----------------------------------------------------------------
            public void Reset()
            {
                GUI.enabled = enable;
                GUI.color = color;
            }

            //-----------------------------------------------------------------
            protected override void CloseScope()
            {
                Reset();
            }
            #endregion Scope
        }

    }
}
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
namespace Prateek.Editors
{
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
    }
}
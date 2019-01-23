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
using Prateek.Helpers;
using Prateek.Attributes;

#if PRATEEK_DEBUG
using Prateek.Debug;
#endif //PRATEEK_DEBUG
#endregion Namespaces

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
            var tempActive = foldoutActive.data;
            if (!isLayout)
            {
                tempActive = EditorGUI.Foldout(rect, foldoutActive.data, content, toggleOnLabelClick);
            }
            else
            {
                tempActive = EditorGUILayout.Foldout(foldoutActive.data, content, toggleOnLabelClick);
            }

            if (EditorGUI.EndChangeCheck())
            {
                foldoutActive.data = tempActive;
            }

            return tempActive;
        }
        #endregion Foldout
    }
}
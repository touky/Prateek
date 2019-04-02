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

#if PRATEEK_DEBUG
using Prateek.Debug;
using static Prateek.Debug.DebugDraw.DebugStyle.QuickCTor;
#endif //PRATEEK_DEBUG
#endregion Prateek

#endregion C# Prateek Namespaces
//
// -END_PRATEEK_CSHARP_NAMESPACE-

//-----------------------------------------------------------------------------
#region File namespaces
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Drawers
{
    //-------------------------------------------------------------------------
    [CustomPropertyDrawer(typeof(EnumBaseAttribute), true)]
    public class EnumBaseAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            var enumAllow = attribute as EnumAllowCategoriesAttribute;
            var enumMask = attribute as EnumMaskAttribute;

            var type = (enumMask == null || enumMask.Value == null) ? fieldInfo.FieldType : enumMask.Value;

            string[] names = Helpers.Editors.GetEnumNames(type, property);
            if (names != null)
            {
                if (enumAllow != null)
                {
                    var contents = new GUIContent[names.Length];
                    for (int i = 0; i < names.Length; i++)
                    {
                        contents[i] = new GUIContent(names[i], names[i]);
                    }
                    property.enumValueIndex = EditorGUI.Popup(rect, content, property.enumValueIndex, contents, EditorStyles.popup);
                }
                else if (enumMask != null)
                {
                    property.intValue = EditorGUI.MaskField(rect, content, property.intValue, names);
                }
            }
        }
    }
}

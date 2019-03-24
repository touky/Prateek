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
using Prateek.ScriptTemplating;
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
using System.Reflection;
#endregion File namespaces

//-----------------------------------------------------------------------------
namespace Prateek.Drawers
{
    //-------------------------------------------------------------------------
    [CustomPropertyDrawer(typeof(TypeRefAttribute), true)]
    public class TypeRefAttributeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent content)
        {
            var popup_shown = false;
            var type_ref = attribute as TypeRefAttribute;
            if (type_ref.value != null && property.propertyType == SerializedPropertyType.String)
            {
                var types = new List<Type>(Assembly.GetAssembly(type_ref.value).GetTypes());
                for (int i = 0; i < types.Count; i++)
                {
                    if (!types[i].IsSubclassOf(type_ref.value)
                      || types[i].FullName.IndexOf('`') != -1
                      || types[i].IsAbstract)
                    {
                        types.RemoveAt(i--);
                    }
                }

                if (types.Count > 0)
                {
                    var names = new GUIContent[types.Count + 1];
                    names[0] = new GUIContent("-");
                    var index = 0;
                    for (int i = 0; i < types.Count; i++)
                    {
                        if (property.stringValue == types[i].FullName)
                        {
                            index = i + 1;
                        }

                        names[i + 1] = new GUIContent(types[i].FullName.Replace('.', '/'));
                    }
                    popup_shown = true;
                    index = EditorGUI.Popup(rect, content, index, names, EditorStyles.popup);
                    property.stringValue = index > 0 ? types[index - 1].FullName : string.Empty;
                }
            }

            if (!popup_shown)
            {
                EditorGUI.PropertyField(rect, property, content, true);
            }
        }
    }
}